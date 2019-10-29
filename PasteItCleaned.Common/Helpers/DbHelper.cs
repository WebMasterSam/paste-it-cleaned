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
                        }

                        return clientObj;
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

        public static ApiKey SelectApiKey(string apiKey)
        {
            return DbHelper.GetApiKey(apiKey);
        }

        public static Client SelectClient(Guid clientId)
        {
            return DbHelper.GetClient(clientId);
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
