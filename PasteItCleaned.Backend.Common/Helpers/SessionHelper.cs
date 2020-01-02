using PasteItCleaned.Common.Entities;
using PasteItCleaned.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Common.Helpers
{
    public static class SessionHelper
    {
        public static Client GetClient(string userName)
        {
            var user = DbHelper.SelectUser(userName);

            if (user == null) {
                DbHelper.InsertUser(userName);
                user = DbHelper.SelectUser(userName);
            }

            var client = DbHelper.GetClient(user.ClientId);

            if (client == null) {
                var apiKey = ApiKeyHelper.GenerateApiKey();

                DbHelper.InsertClient(user.ClientId, apiKey);
                DbHelper.InsertApiKey(apiKey);

                client = DbHelper.GetClient(user.ClientId);
            }

            return client;
        }
    }
}
