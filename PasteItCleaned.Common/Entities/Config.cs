using System;
using System.Collections.Generic;

namespace PasteItCleaned.Common.Entities
{
    public class Config
    {
        public Guid ClientId { get; set; }
        public DateTime ExpiresOn { get; set; }

        public string Name { get; set; }
        public Dictionary<string, bool> Common { get; set; }
        public Dictionary<string, bool> Office { get; set; }
        public Dictionary<string, bool> Web { get; set; }

        public bool GetConfigValue(string name, bool defaultValue)
        {
            var common = GetConfigValue(this.Common, name);
            var office = GetConfigValue(this.Office, name);
            var web = GetConfigValue(this.Web, name);

            if (common.HasValue)
                return common.Value;

            if (office.HasValue)
                return office.Value;

            if (web.HasValue)
                return web.Value;

            return defaultValue;
        }

        private bool? GetConfigValue(Dictionary<string, bool> dic, string name)
        {
            var value = (bool?)null;

            foreach (var ke in dic)
                if (ke.Key.ToLower().Trim() == name.ToLower().Trim())
                    value = ke.Value;

            return value;
        }
    }
}
