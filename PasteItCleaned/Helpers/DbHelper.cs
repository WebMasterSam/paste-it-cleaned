﻿using System;
using System.Collections;
using System.Collections.Generic;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using PasteItCleaned.Cleaners;
using PasteItCleaned.Entities;

namespace PasteItCleaned.Helpers
{
    public static class DbHelper
    {
        private static Hashtable _tables = new Hashtable();


        public static void InsertError(Exception ex)
        {
            DbHelper.SaveError(ex);
        }

        public static void InsertHit(Guid clientId, SourceType type)
        {
            var price = AccountHelper.GetHitPrice(clientId, type);

            DbHelper.SaveHit(clientId, type, price);
            DbHelper.SaveHitDaily(clientId, type, price);
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
                    { "clientid", new AttributeValue { S = key } }
                };

                UpdateItemRequest updateClientBalance = new UpdateItemRequest()
                {
                    TableName = tableClient,
                    Key = dbKey,
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":decreaseBy", new AttributeValue { N = decreaseBy.ToString(System.Globalization.CultureInfo.InvariantCulture) } }
                    },
                    UpdateExpression = "SET billing.balance = billing.balance - :decreaseBy",
                    ReturnValues = "NONE"
                };

                client.UpdateItemAsync(updateClientBalance);
            }
        }


        private static void SaveHit(Guid clientId, SourceType type, decimal price)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tablesHit = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Hit");
                var key = string.Format("{0}-{1}", clientId, type);

                var insertHit = new PutItemRequest
                {
                    TableName = tablesHit,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "key", new AttributeValue { S = key }},
                        { "timestamp", new AttributeValue { S = DateTime.Now.ToUniversalTime().ToString("o") }},
                        { "price", new AttributeValue { N = price.ToString(System.Globalization.CultureInfo.InvariantCulture) }}
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
                var key = string.Format("{0}-{1}", clientId, type);
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "key", new AttributeValue { S = key } },
                    { "date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") } }
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
                        var count = Convert.ToInt64(item["count"].N);
                        var totalPrice = decimal.Parse(item["price"].N, System.Globalization.CultureInfo.InvariantCulture);

                        var updateHit = new PutItemRequest
                        {
                            TableName = tablesHitDaily,
                            Item = new Dictionary<string, AttributeValue>
                            {
                                { "key", new AttributeValue { S = key } },
                                { "date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") } },
                                { "count", new AttributeValue { N = (count + 1).ToString("N0") } },
                                { "price", new AttributeValue { N = (totalPrice + price).ToString(System.Globalization.CultureInfo.InvariantCulture) }}
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
                                { "key", new AttributeValue { S = key }},
                                { "date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") }},
                                { "count", new AttributeValue { N = "1" }},
                                { "price", new AttributeValue { N = price.ToString(System.Globalization.CultureInfo.InvariantCulture) }}
                            }
                        };

                        client.PutItemAsync(insertHit);
                    }
                }
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
                        { "timestamp", new AttributeValue { S = DateTime.Now.ToUniversalTime().ToString("o") }},
                        { "message", new AttributeValue { S = ex.Message }},
                        { "stackTrace", new AttributeValue { S = ex.StackTrace }}
                    }
                };

                client.PutItemAsync(insertError);
            }
        }


        private static ApiKey GetApiKey(string apiKey)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableApiKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.ApiKey");
                var key = apiKey;
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "apikey", new AttributeValue { S = key } }
                };

                var selectApiKey = new GetItemRequest
                {
                    TableName = tableApiKey,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectApiKey);

                asyncGet.Wait(1000);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var apiKeyObj = new ApiKey();

                        apiKeyObj.Key = item["apikey"].S;
                        apiKeyObj.ClientId = new Guid(item["clientId"].S);
                        apiKeyObj.ExpiresOn = DateTime.Parse(item["expiresOn"].S);
                        apiKeyObj.Domains = new List<string>();

                        foreach (var domain in item["domains"].L)
                            apiKeyObj.Domains.Add(domain.S);

                        return apiKeyObj;
                    }
                }
            }

            return null;
        }

        private static Client GetClient(Guid clientId)
        {
            using (var client = DbHelper.GetAmazonClient())
            {
                var tableClient = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Client");
                var key = clientId.ToString();
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "clientid", new AttributeValue { S = key } }
                };

                var selectClient = new GetItemRequest
                {
                    TableName = tableClient,
                    Key = dbKey
                };

                var asyncGet = client.GetItemAsync(selectClient);

                asyncGet.Wait(1000);

                if (asyncGet.IsCompletedSuccessfully)
                {
                    var item = asyncGet.Result.Item;

                    if (item != null && item.Count > 0)
                    {
                        var clientObj = new Client();
                        var billing = item["billing"].M;
                        var billingBalance = GetMapNode(billing, "balance");
                        var business = item["business"].M;
                        var businessContact = GetMapNode(business, "contact");
                        var contact = item["contact"].M;

                        clientObj.ClientId = new Guid(item["clientid"].S);

                        clientObj.Contact = new Contact();
                        clientObj.Contact.FirstName = contact["firstName"].S;
                        clientObj.Contact.LastName = contact["lastName"].S;
                        clientObj.Contact.PhoneNumber = contact["phoneNumber"].S;

                        clientObj.Business = new Business();
                        clientObj.Business.Name = business["name"].S;

                        clientObj.Business.Contact = new Contact();
                        clientObj.Business.Contact.FirstName = contact["firstName"].S;
                        clientObj.Business.Contact.LastName = contact["lastName"].S;
                        clientObj.Business.Contact.Address = businessContact.M["address"].S;
                        clientObj.Business.Contact.City = businessContact.M["city"].S;
                        clientObj.Business.Contact.Country = businessContact.M["country"].S;
                        clientObj.Business.Contact.PhoneNumber = businessContact.M["phoneNumber"].S;
                        clientObj.Business.Contact.State = businessContact.M["state"].S;

                        clientObj.ApiKeys = new List<string>();
                        foreach (var apiKey in item["apiKeys"].SS)
                            clientObj.ApiKeys.Add(apiKey);

                        clientObj.Billing = new Billing();
                        clientObj.Billing.Balance = decimal.Parse(billingBalance.N, System.Globalization.CultureInfo.InvariantCulture);
                        clientObj.Billing.Contact = new Contact();
                        clientObj.Billing.Contact.FirstName = contact["firstName"].S;
                        clientObj.Billing.Contact.LastName = contact["lastName"].S;
                        clientObj.Billing.Contact.PhoneNumber = contact["phoneNumber"].S;

                        clientObj.Configs = new List<Config>();
                        foreach (var config in item["configs"].L)
                        {
                            var obj = new Config();
                            var common = GetMapNode(config.M, "common");
                            var office = GetMapNode(config.M, "office");
                            var web = GetMapNode(config.M, "web");

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
