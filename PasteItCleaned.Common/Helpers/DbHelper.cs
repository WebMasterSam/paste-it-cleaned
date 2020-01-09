using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using PasteItCleaned.Common.Cleaners;
using PasteItCleaned.Common.Entities;
using PasteItCleaned.Core.Helpers;

namespace PasteItCleaned.Common.Helpers
{
    public static class DbHelper
    {
        private static Hashtable _tables = new Hashtable();


        public static ApiKey GetApiKey(string apiKey)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableApiKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.ApiKey");
                var key = apiKey;
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "ApiKey", new AttributeValue { S = key } }
                };

                var selectApiKey = new GetItemRequest
                {
                    TableName = tableApiKey,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectApiKey);

                WaitAndCatch(asyncGet);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var apiKeyObj = new ApiKey();

                        apiKeyObj.Key = item["ApiKey"].S;
                        apiKeyObj.ClientId = new Guid(item["ClientId"].S);
                        apiKeyObj.ExpiresOn = DateTime.Parse(item["ExpiresOn"].S);
                        apiKeyObj.Domains = new List<string>();

                        foreach (var domain in item["Domains"].L)
                            apiKeyObj.Domains.Add(domain.S);

                        return apiKeyObj;
                    }
                }
            }

            return null;
        }

        public static Client GetClient(Guid clientId)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableClient = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Client");
                var key = clientId.ToString();
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "ClientId", new AttributeValue { S = key } }
                };

                var selectClient = new GetItemRequest
                {
                    TableName = tableClient,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectClient);

                WaitAndCatch(asyncGet);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var clientObj = new Client();
                        var billing = item["Billing"].M;
                        var billingBalance = GetMapNode(billing, "Balance");
                        var business = item["Business"].M;
                        var businessContact = GetMapNode(business, "Contact");
                        var contact = item["Contact"].M;

                        clientObj.ClientId = new Guid(item["ClientId"].S);

                        clientObj.Contact = new Contact();
                        clientObj.Contact.FirstName = contact["FirstName"].S;
                        clientObj.Contact.LastName = contact["LastName"].S;
                        clientObj.Contact.PhoneNumber = contact["PhoneNumber"].S;

                        clientObj.Business = new Business();
                        clientObj.Business.Name = business["Name"].S;

                        clientObj.ApiKeys = new List<string>();

                        foreach (var apiKey in item["ApiKeys"].SS)
                            clientObj.ApiKeys.Add(apiKey);

                        clientObj.Billing = new Billing();
                        clientObj.Billing.Balance = decimal.Parse(billingBalance.N, System.Globalization.CultureInfo.InvariantCulture);
                        clientObj.Billing.Contact = new Contact();
                        clientObj.Billing.Contact.FirstName = contact["FirstName"].S;
                        clientObj.Billing.Contact.LastName = contact["LastName"].S;
                        clientObj.Billing.Contact.PhoneNumber = contact["PhoneNumber"].S;

                        clientObj.Configs = new List<Config>();

                        foreach (var config in item["Configs"].L)
                        {
                            var obj = new Config();
                            var common = GetMapNode(config.M, "Common");
                            var office = GetMapNode(config.M, "Office");
                            var web = GetMapNode(config.M, "Web");

                            obj.Name = config.M["Name"].S;
                            obj.Common = new Dictionary<string, bool>();
                            obj.Office = new Dictionary<string, bool>();
                            obj.Web = new Dictionary<string, bool>();

                            foreach (var i in common.M)
                                obj.Common.Add(i.Key, i.Value.BOOL);

                            foreach (var i in office.M)
                                obj.Office.Add(i.Key, i.Value.BOOL);

                            foreach (var i in web.M)
                                obj.Web.Add(i.Key, i.Value.BOOL);

                            clientObj.Configs.Add(obj);
                        }

                        return clientObj;
                    }
                }
            }

            return null;
        }

        public static User GetUser(string userName)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableUser = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.User");
                var key = userName;
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue { S = key } }
                };

                var selectUser = new GetItemRequest
                {
                    TableName = tableUser,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectUser);

                WaitAndCatch(asyncGet);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var userObj = new User();

                        userObj.UserName = item["UserId"].S;
                        userObj.ClientId = new Guid(item["ClientId"].S);

                        return userObj;
                    }
                }
            }

            return null;
        }

        public static DateTime GetHitHash(Guid clientId, string hash)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tablesHitHash = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.HitHash");
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "ClientId", new AttributeValue { S = clientId.ToString() }},
                    { "Hash", new AttributeValue { S = hash } }
                };

                var selectHitHash = new GetItemRequest
                {
                    TableName = tablesHitHash,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectHitHash);

                WaitAndCatch(asyncGet);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var timestamp = item["Timestamp"].S;

                        return DateTime.Parse(timestamp);
                    }
                }

                return DateTime.MinValue;
            }
        }


        public static void InsertError(Exception ex)
        {
            DbHelper.SaveError(ex);
        }

        public static void InsertHit(Guid clientId, SourceType type, string ip, string referer, decimal price)
        {
            DbHelper.SaveHit(clientId, type, price, ip, referer);
            DbHelper.SaveHitDaily(clientId, type, price);
        }

        public static void InsertHitHash(Guid clientId, string hash)
        {
            DbHelper.SaveHitHash(clientId, hash);
        }

        public static void InsertUser(string userName)
        {
            DbHelper.SaveUser(userName, Guid.NewGuid());
        }

        public static void InsertClient(Guid clientId, string apiKey)
        {
            var clientObj = new Client();

            clientObj.ClientId = clientId;

            clientObj.Contact = new Contact();
            clientObj.Business = new Business();
            clientObj.ApiKeys = new List<string>();
            clientObj.ApiKeys.Add(apiKey);
            clientObj.Billing = new Billing();
            clientObj.Billing.Contact = new Contact();
            clientObj.Billing.PayPal = new BillingPayPal();
            clientObj.Billing.Stripe = new BillingStripe();
            clientObj.Configs = new List<Config>();
            clientObj.Configs.Add(new Config() { Name = "DEFAULT", Common = new Dictionary<string, bool>() { { "EmbedExternalImages", false }, { "RemoveEmptyTags", true }, { "RemoveSpanTags", true } }, Web = new Dictionary<string, bool>() { { "RemoveClassNames", true }, { "RemoveIframes", true }, { "RemoveTagAttributes", true } } });

            DbHelper.SaveClient(clientObj);
        }

        public static void InsertApiKey(Guid clientId, string apiKey)
        {
            ApiKey key = new ApiKey();

            key.ClientId = clientId;
            key.Key = apiKey;
            key.Domains = new List<string>();
            key.Domains.Add("yourdomain.com");
            key.ExpiresOn = DateTime.Now.AddYears(1);

            DbHelper.SaveApiKey(key);
        }


        public static ApiKey SelectApiKey(string apiKey)
        {
            return DbHelper.GetApiKey(apiKey);
        }

        public static Client SelectClient(Guid clientId)
        {
            return DbHelper.GetClient(clientId);
        }

        public static User SelectUser(string userName)
        {
            return DbHelper.GetUser(userName);
        }


        public static void DecreaseClientBalance(Guid clientId, decimal decreaseBy)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableClient = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Client");
                var key = clientId.ToString();
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "ClientId", new AttributeValue { S = key } }
                };

                UpdateItemRequest updateClientBalance = new UpdateItemRequest()
                {
                    TableName = tableClient,
                    Key = dbKey,
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":decreaseBy", new AttributeValue { N = decreaseBy.ToString(System.Globalization.CultureInfo.InvariantCulture) } }
                    },
                    UpdateExpression = "SET Billing.Balance = Billing.Balance - :decreaseBy",
                    ReturnValues = "NONE"
                };

                client.UpdateItemAsync(updateClientBalance);
            }
        }







        private static void SaveHit(Guid clientId, SourceType type, decimal price, string ip, string referer)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tablesHit = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Hit");

                var insertHit = new PutItemRequest
                {
                    TableName = tablesHit,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "ClientId", new AttributeValue { S = clientId.ToString() }},
                        { "Timestamp", new AttributeValue { S = DateTime.Now.ToUniversalTime().ToString("o") }},
                        { "Price", new AttributeValue { N = price.ToString(System.Globalization.CultureInfo.InvariantCulture) }},
                        { "Type", new AttributeValue { S = type.ToString() }},
                        { "IP", new AttributeValue { S = ip }},
                        { "Referer", new AttributeValue { S = referer }}
                    }
                };

                WaitAndCatch(client.PutItemAsync(insertHit));
            }
        }

        private static void SaveHitDaily(Guid clientId, SourceType type, decimal price)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tablesHitDaily = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.HitDaily");
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "ClientId", new AttributeValue { S = clientId.ToString() }},
                    { "Date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") } }
                };

                var selectHit = new GetItemRequest
                {
                    TableName = tablesHitDaily,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectHit);

                WaitAndCatch(asyncGet);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var counts = item["Counts"].M;
                        var totalPrice = decimal.Parse(item["Price"].N, System.Globalization.CultureInfo.InvariantCulture);
                        var countFound = false;

                        foreach (var count in counts)
                            if (count.Key.ToLower() == type.ToString().ToLower())
                                countFound = true;

                        if (!countFound)
                            counts.Add( type.ToString(), new AttributeValue { N = "1" } );

                        foreach (var count in counts)
                            if (count.Key.ToLower() == type.ToString().ToLower())
                                count.Value.N = (int.Parse(count.Value.N) + 1).ToString();

                        var updateHit = new PutItemRequest
                        {
                            TableName = tablesHitDaily,
                            Item = new Dictionary<string, AttributeValue>
                            {
                                { "ClientId", new AttributeValue { S = clientId.ToString() }},
                                { "Date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") } },
                                { "Price", new AttributeValue { N = (totalPrice + price).ToString(System.Globalization.CultureInfo.InvariantCulture) }},
                                { "Counts", new AttributeValue { M = counts }}
                            }
                        };

                        WaitAndCatch(client.PutItemAsync(updateHit));
                    }
                    else
                    {
                        var insertHit = new PutItemRequest
                        {
                            TableName = tablesHitDaily,
                            Item = new Dictionary<string, AttributeValue>
                            {
                                { "ClientId", new AttributeValue { S = clientId.ToString() }},
                                { "Date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") }},
                                { "Price", new AttributeValue { N = price.ToString(System.Globalization.CultureInfo.InvariantCulture) }},
                                { "Counts", new AttributeValue { M = new Dictionary<string, AttributeValue>
                                    {
                                        {  type.ToString(), new AttributeValue { N = "1" } }
                                    }
                                }}
                            }
                        };

                        WaitAndCatch(client.PutItemAsync(insertHit));
                    }
                }
            }
        }

        private static void SaveHitHash(Guid clientId, string hash)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tablesHitHash = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.HitHash");

                var insertHitHash = new PutItemRequest
                {
                    TableName = tablesHitHash,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "ClientId", new AttributeValue { S = clientId.ToString() }},
                        { "Hash", new AttributeValue { S = hash }},
                        { "Timestamp", new AttributeValue { S = DateTime.Now.ToUniversalTime().ToString("o") }}
                    }
                };

                WaitAndCatch(client.PutItemAsync(insertHitHash));
            }
        }

        private static void SaveError(Exception ex)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableError = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Error");

                var insertError = new PutItemRequest
                {
                    TableName = tableError,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "Timestamp", new AttributeValue { S = DateTime.Now.ToUniversalTime().ToString("o") }},
                        { "Message", new AttributeValue { S = ex.Message }},
                        { "StackTrace", new AttributeValue { S = ex.StackTrace }}
                    }
                };

                WaitAndCatch(client.PutItemAsync(insertError));
            }
        }

        private static void SaveUser(string userName, Guid clientId)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableUser = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.User");

                var insertUser = new PutItemRequest
                {
                    TableName = tableUser,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "UserId", new AttributeValue { S = userName }},
                        { "ClientId", new AttributeValue { S = clientId.ToString() }}
                    }
                };

                WaitAndCatch(client.PutItemAsync(insertUser));
            }
        }

        private static void SaveClient(Client clientObj)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableClient = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Client");
                var billing = new Dictionary<string, AttributeValue>();
                var billingContact = new Dictionary<string, AttributeValue>();
                var billingPayPal = new Dictionary<string, AttributeValue>();
                var billingStripe = new Dictionary<string, AttributeValue>();
                var business = new Dictionary<string, AttributeValue>();
                var configs = new List<AttributeValue>();
                var contact = new Dictionary<string, AttributeValue>();

                AddSAttributeValue(billingContact, "Address", clientObj.Billing.Contact.Address);
                AddSAttributeValue(billingContact, "City", clientObj.Billing.Contact.City);
                AddSAttributeValue(billingContact, "Country", clientObj.Billing.Contact.Country);
                AddSAttributeValue(billingContact, "FirstName", clientObj.Billing.Contact.FirstName);
                AddSAttributeValue(billingContact, "LastName", clientObj.Billing.Contact.LastName);
                AddSAttributeValue(billingContact, "PhoneNumber", clientObj.Billing.Contact.PhoneNumber);
                AddSAttributeValue(billingContact, "State", clientObj.Billing.Contact.State);

                AddSAttributeValue(billingPayPal, "Token", clientObj.Billing.PayPal.Token);

                AddSAttributeValue(billingStripe, "Token", clientObj.Billing.Stripe.Token);

                //billing.Add("Balance", new AttributeValue { N = "0.00" });
                billing.Add("Contact", new AttributeValue { M = billingContact });
                billing.Add("PayPal", new AttributeValue { M = billingPayPal });
                billing.Add("Stripe", new AttributeValue { M = billingStripe });

                business.Add("Name", SAttributeValue(clientObj.Business.Name));

                foreach (Config config in clientObj.Configs)
                {
                    var oneConfig = new Dictionary<string, AttributeValue>();
                    var configCommon = new Dictionary<string, AttributeValue>();
                    var configOffice = new Dictionary<string, AttributeValue>();
                    var configWeb = new Dictionary<string, AttributeValue>();

                    configCommon.Add("EmbedExternalImages", new AttributeValue { BOOL = config.Common["EmbedExternalImages"] });
                    configCommon.Add("RemoveEmptyTags", new AttributeValue { BOOL = config.Common["RemoveEmptyTags"] });
                    configCommon.Add("RemoveSpanTags", new AttributeValue { BOOL = config.Common["RemoveSpanTags"] });

                    configWeb.Add("RemoveClassNames", new AttributeValue { BOOL = config.Web["RemoveClassNames"] });
                    configWeb.Add("RemoveIframes", new AttributeValue { BOOL = config.Web["RemoveIframes"] });
                    configWeb.Add("RemoveTagAttributes", new AttributeValue { BOOL = config.Web["RemoveTagAttributes"] });

                    oneConfig.Add("Common", new AttributeValue { M = configCommon });
                    oneConfig.Add("Office", new AttributeValue { M = configOffice });
                    oneConfig.Add("Web", new AttributeValue { M = configWeb });

                    configs.Add(new AttributeValue { M = oneConfig });
                }

                AddSAttributeValue(contact, "Address", clientObj.Contact.Address);
                AddSAttributeValue(contact, "City", clientObj.Contact.City);
                AddSAttributeValue(contact, "Country", clientObj.Contact.Country);
                AddSAttributeValue(contact, "FirstName", clientObj.Contact.FirstName);
                AddSAttributeValue(contact, "LastName", clientObj.Contact.LastName);
                AddSAttributeValue(contact, "PhoneNumber", clientObj.Contact.PhoneNumber);
                AddSAttributeValue(contact, "State", clientObj.Contact.State);

                var insertClient = new PutItemRequest
                {
                    TableName = tableClient,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "ClientId", SAttributeValue(clientObj.ClientId.ToString())},
                        { "ApiKeys", new AttributeValue { SS = clientObj.ApiKeys }},
                        { "Billing", new AttributeValue { M = billing }},
                        { "Business", new AttributeValue { M = business }},
                        { "Configs", new AttributeValue { L = configs }},
                        { "Contact", new AttributeValue { M = contact }}
                    }
                };

                Console.WriteLine(JsonConvert.SerializeObject(insertClient));

                WaitAndCatch(client.PutItemAsync(insertClient));
            }
        }

        private static void SaveApiKey(ApiKey apiKey)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableApiKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.ApiKey");
                var domains = new List<AttributeValue>();

                foreach (string domain in apiKey.Domains)
                {
                    domains.Add(new AttributeValue { S = domain });
                }

                var insertApiKey = new PutItemRequest
                {
                    TableName = tableApiKey,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "ApiKey", new AttributeValue { S = apiKey.Key }},
                        { "ClientId", new AttributeValue { S = apiKey.ClientId.ToString() }},
                        { "Domains", new AttributeValue { L = domains }},
                        { "ExpiresOn", new AttributeValue { S = apiKey.ExpiresOn.ToString() }}
                    }
                };

                client.PutItemAsync(insertApiKey);
            }
        }


        private static void WaitAndCatch<T>(Task<T> task)
        {
            try
            {
                try
                {
                    task.Wait(5000);
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
            }
            catch (AmazonServiceException ase)
            {
                Console.WriteLine("Could not complete operation");
                Console.WriteLine("Error Message:  " + ase.Message);
                Console.WriteLine("HTTP Status:    " + ase.StatusCode);
                Console.WriteLine("AWS Error Code: " + ase.ErrorCode);
                Console.WriteLine("Error Type:     " + ase.ErrorType);
                Console.WriteLine("Request ID:     " + ase.RequestId);
            }
            catch (AmazonClientException ace)
            {
                Console.WriteLine("Internal error occurred communicating with DynamoDB");
                Console.WriteLine("Error Message:  " + ace.Message);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException.Message + " " + ex.InnerException.StackTrace);
                throw;
            }
        }

        private static AttributeValue GetMapNode(Dictionary<string, AttributeValue> map, string node)
        {
            foreach (var o in map)
                if (o.Key == node)
                    return o.Value;

            return new AttributeValue();
        }

        private static AmazonDynamoDBClient GetAmazonClient()
        {
            var accessKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.AccessKey");
            var secretKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.SecretKey");

            return new AmazonDynamoDBClient(accessKey, secretKey, Amazon.RegionEndpoint.USEast1);
        }

        private static AttributeValue SAttributeValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new AttributeValue { S = "-" };

            return new AttributeValue { S = value };
        }

        private static void AddSAttributeValue(Dictionary<string, AttributeValue> parent, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                parent.Add(name, SAttributeValue(value));
        }
    }
}
