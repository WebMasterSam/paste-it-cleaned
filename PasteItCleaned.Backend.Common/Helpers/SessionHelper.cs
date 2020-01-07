using Newtonsoft.Json.Linq;
using PasteItCleaned.Common.Entities;
using PasteItCleaned.Common.Helpers;
using System;

namespace PasteItCleaned.Backend.Common.Helpers
{
    public static class SessionHelper
    {
        public static Client GetCurrentClient(string authToken)
        {
            Console.WriteLine(authToken);

            if (!string.IsNullOrWhiteSpace(authToken))
            {
                var accessToken = Base64Helper.GetString(authToken.Replace("Bearer ", "").Split('.')[1]);
                var token = JObject.Parse(accessToken);
                var userName = token.SelectToken("username").Value<string>();

                return GetClient(userName, userName);
            }

            return null;
        }

        public static Client GetClient(string cognitoId, string cognitoUserName)
        {
            var user = DbHelper.SelectUser(cognitoUserName);

            if (user == null) {
                DbHelper.InsertUser(cognitoUserName);
                user = DbHelper.SelectUser(cognitoUserName);
            }

            var client = DbHelper.GetClient(user.ClientId);

            if (client == null) {
                var apiKey = ApiKeyHelper.GenerateApiKey();

                DbHelper.InsertClient(user.ClientId, apiKey);
                DbHelper.InsertApiKey(user.ClientId, apiKey);

                client = DbHelper.GetClient(user.ClientId);
            }

            return client;
        }
    }
}
