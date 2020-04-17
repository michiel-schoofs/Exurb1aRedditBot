using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace RedditBot.Models.Domain {
    public class Secrets {
        public string DiscordSecretID { get; set; }
        public string RedditAppID { get; set; }
        public string RedditAppSecret { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

        public Secrets(IConfigurationSection section) {
            foreach (IConfigurationSection kv in section.GetChildren()) {
                PropertyInfo info = GetType().GetProperty(kv.Key);
                if (info != null) {
                    info.SetValue(this,kv.Value,null);
                }
            }
        }
    }
}
