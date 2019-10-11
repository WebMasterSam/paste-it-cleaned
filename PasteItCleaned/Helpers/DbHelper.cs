using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            DbHelper.SaveHit(clientId, type);
            DbHelper.SaveHitDaily(clientId, type);
        }

        public static ApiKey SelectApiKey(string apiKey)
        {
            return DbHelper.GetApiKey(apiKey);
        }


        private static void SaveHit(Guid clientId, SourceType type)
        {
            using (var client = DbHelper.GetClient())
            {
                var tablesHit = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.Hit");
                var key = string.Format("{0}-{1}", clientId, type);

                var insertHit = new PutItemRequest
                {
                    TableName = tablesHit,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "key", new AttributeValue { S = key }},
                        { "timestamp", new AttributeValue { S = DateTime.Now.ToUniversalTime().ToString("o") }}
                    }
                };

                client.PutItemAsync(insertHit);
            }
        }

        private static void SaveHitDaily(Guid clientId, SourceType type)
        {
            using (var client = DbHelper.GetClient())
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

                        var updateHit = new PutItemRequest
                        {
                            TableName = tablesHitDaily,
                            Item = new Dictionary<string, AttributeValue>
                            {
                                { "key", new AttributeValue { S = key } },
                                { "date", new AttributeValue { S = DateTime.Today.ToUniversalTime().ToString("o") } },
                                { "count", new AttributeValue { N = (count + 1).ToString("N0") } }
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
                                { "count", new AttributeValue { N = "1" }}
                            }
                        };

                        client.PutItemAsync(insertHit);
                    }
                }
            }
        }

        private static void SaveError(Exception ex)
        {
            using (var client = DbHelper.GetClient())
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
            using (var client = DbHelper.GetClient())
            {
                var tableApiKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.Tables.ApiKey");
                var key = apiKey;
                var dbKey = new Dictionary<string, AttributeValue>
                {
                    { "apiKey", new AttributeValue { S = key } }
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
                        var count = Convert.ToInt64(item["count"].N);
                        var apiKeyObj = new ApiKey();

                        apiKeyObj.Key = item["apikey"].S;
                        apiKeyObj.ClientId = new Guid(item["count"].N);
                        apiKeyObj.ExpiresOn = DateTime.Parse(item["expiresOn"].S);

                        return apiKeyObj;
                    }
                }
            }

            return null;
        }

        private static AmazonDynamoDBClient GetClient()
        {
            var accessKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.AccessKey");
            var secretKey = ConfigHelper.GetAppSetting("Amazon.DynamoDB.SecretKey");

            return new AmazonDynamoDBClient(accessKey, secretKey, Amazon.RegionEndpoint.USEast1);
        }
    }
}
