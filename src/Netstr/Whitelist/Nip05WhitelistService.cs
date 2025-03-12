using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Netstr.Whitelist
{
    public class Nip05WhitelistService
    {
        private readonly HttpClient httpClient;
        private readonly string[] domains;
        private readonly bool isEnabled;

        public Nip05WhitelistService(string[] domains, bool isEnabled)
        {
            this.httpClient = new HttpClient();
            this.domains = domains;
            this.isEnabled = isEnabled;
        }

        public async Task<bool> IsUserWhitelisted(string userPublicKey)
        {
            if (!isEnabled) return true;

            foreach (var domain in domains)
            {
                var response = await httpClient.GetStringAsync(domain);
                var whitelist = JsonSerializer.Deserialize<string[]>(response);
                if (whitelist != null && whitelist.Contains(userPublicKey))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetErrorMessage()
        {
            return "You are not whitelisted. Please pay to gain access.";
        }
    }
} 