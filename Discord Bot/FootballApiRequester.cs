using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Discord_Bot
{
    public static class FootballApiRequester
    {
#pragma warning disable CS8618

        private const string fifaUri = "https://v3.football.api-sports.io/";
        public static HttpClient apiClient;

        public static void InitializeClient(IConfiguration configuration)
        {
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(fifaUri);
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Add("x-rapidapi-key", configuration["FootballApiToken"]);
            apiClient.DefaultRequestHeaders.Add("x-rapidapi-host", "v3.football.api-sports.io");
        }
    }
}
