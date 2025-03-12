using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Netstr.Whitelist
{
    public class Nip05WhitelistService
    {
        private readonly HttpClient httpClient;
        private readonly string[] domains;
        private readonly bool isEnabled;
        private readonly ILogger<Nip05WhitelistService> logger;

        public Nip05WhitelistService(string[] domains, bool isEnabled, ILogger<Nip05WhitelistService> logger)
        {
            this.httpClient = new HttpClient();
            this.domains = domains;
            this.isEnabled = isEnabled;
            this.logger = logger;
        }

        public async Task<bool> IsUserWhitelisted(string publicKey)
        {
            if (!isEnabled) return true;

            this.logger.LogInformation("Fetching the whitelist...");

            foreach (var domain in domains)
            {
                var response = await httpClient.GetStringAsync(domain);
                var whitelist = JsonSerializer.Deserialize<string[]>(response);
                if (whitelist != null && whitelist.Contains(publicKey))
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