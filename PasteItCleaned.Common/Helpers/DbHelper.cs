using System;
using System.Collections;
using System.Collections.Generic;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using PasteItCleaned.Common.Cleaners;
using PasteItCleaned.Common.Entities;

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

                asyncGet.Wait(5000);

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

                asyncGet.Wait(5000);

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

                asyncGet.Wait(5000);

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

                asyncGet.Wait(5000);

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
            clientObj.Configs = new List<Config>();
            clientObj.Configs.Add(new Config() { Name = "DEFAULT", Common = new Dictionary<string, bool>() { { "EmbedExternalImages", false }, { "RemoveEmptyTags", true }, { "RemoveSpanTags", true } }, Web = new Dictionary<string, bool>() { { "RemoveClassNames", true }, { "RemoveIframes", true }, { "RemoveTagAttributes", true } } });

            DbHelper.SaveClient(clientObj);
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

                client.PutItemAsync(insertHit);
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

                asyncGet.Wait(1000);

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
                        
                        client.PutItemAsync(updateHit);
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

                        client.PutItemAsync(insertHit);
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

                client.PutItemAsync(insertHitHash);
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

                client.PutItemAsync(insertError);
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

                client.PutItemAsync(insertUser);
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

                billingContact.Add("Address", new AttributeValue { S = clientObj.Billing.Contact.Address });
                billingContact.Add("City", new AttributeValue { S = clientObj.Billing.Contact.City });
                billingContact.Add("Country", new AttributeValue { S = clientObj.Billing.Contact.Country });
                billingContact.Add("FirstName", new AttributeValue { S = clientObj.Billing.Contact.FirstName });
                billingContact.Add("LastName", new AttributeValue { S = clientObj.Billing.Contact.LastName });
                billingContact.Add("PhoneNumber", new AttributeValue { S = clientObj.Billing.Contact.PhoneNumber });
                billingContact.Add("State", new AttributeValue { S = clientObj.Billing.Contact.State });

                billingPayPal.Add("Token", new AttributeValue { S = clientObj.Billing.PayPal.Token });

                billingStripe.Add("Token", new AttributeValue { S = clientObj.Billing.Stripe.Token });

                billing.Add("Balance", new AttributeValue { N = "0" });
                billing.Add("Contact", new AttributeValue { M = billingContact });
                billing.Add("PayPal", new AttributeValue { M = billingPayPal });
                billing.Add("Stripe", new AttributeValue { M = billingStripe });

                business.Add("Name", new AttributeValue { S = clientObj.Business.Name });

                foreach (Config config in clientObj.Configs)
                {
                    var oneConfig = new Dictionary<string, AttributeValue>();
                    var configCommon = new Dictionary<string, AttributeValue>();
                    var configOffice = new Dictionary<string, AttributeValue>();
                    var configWeb = new Dictionary<string, AttributeValue>();

                    configCommon.Add("EmbedExternalImages", new AttributeValue { BOOL = config.Common["EmbedExternalImages"] });
                    configCommon.Add("RemoveEmptyTags", new AttributeValue { BOOL = config.Common["RemoveEmptyTags"] });
                    configCommon.Add("RemoveSpanTags", new AttributeValue { BOOL = config.Common["RemoveSpanTags"] });

                    configWeb.Add("RemoveClassNames", new AttributeValue { BOOL = config.Common["RemoveClassNames"] });
                    configWeb.Add("RemoveIframes", new AttributeValue { BOOL = config.Common["RemoveIframes"] });
                    configWeb.Add("RemoveTagAttributes", new AttributeValue { BOOL = config.Common["RemoveTagAttributes"] });

                    oneConfig.Add("Common", new AttributeValue { M = configCommon });
                    oneConfig.Add("Office", new AttributeValue { M = configOffice });
                    oneConfig.Add("Web", new AttributeValue { M = configWeb });

                    configs.Add(new AttributeValue { M = oneConfig });
                }

                contact.Add("Address", new AttributeValue { S = clientObj.Contact.Address });
                contact.Add("City", new AttributeValue { S = clientObj.Contact.City });
                contact.Add("Country", new AttributeValue { S = clientObj.Contact.Country });
                contact.Add("FirstName", new AttributeValue { S = clientObj.Contact.FirstName });
                contact.Add("LastName", new AttributeValue { S = clientObj.Contact.LastName });
                contact.Add("PhoneNumber", new AttributeValue { S = clientObj.Contact.PhoneNumber });
                contact.Add("State", new AttributeValue { S = clientObj.Contact.State });

                var insertClient = new PutItemRequest
                {
                    TableName = tableClient,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "ClientId", new AttributeValue { S = clientObj.ClientId.ToString() }},
                        { "ApiKeys", new AttributeValue { SS = clientObj.ApiKeys }},
                        { "Billing", new AttributeValue { M = billing }},
                        { "Business", new AttributeValue { M = business }},
                        { "Configs", new AttributeValue { L = configs }},
                        { "Contact", new AttributeValue { M = contact }}
                    }
                };

                /*var billing = item["Billing"].M;
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

                clientObj.Business.Contact = new Contact();
                clientObj.Business.Contact.FirstName = contact["FirstName"].S;
                clientObj.Business.Contact.LastName = contact["LastName"].S;
                clientObj.Business.Contact.Address = businessContact.M["Address"].S;
                clientObj.Business.Contact.City = businessContact.M["City"].S;
                clientObj.Business.Contact.Country = businessContact.M["Country"].S;
                clientObj.Business.Contact.PhoneNumber = businessContact.M["PhoneNumber"].S;
                clientObj.Business.Contact.State = businessContact.M["State"].S;

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
                }*/

                client.PutItemAsync(insertClient);
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
    }
}
