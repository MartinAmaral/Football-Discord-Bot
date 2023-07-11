using Models;
using Newtonsoft.Json;
using System.Numerics;

namespace Discord_Bot
{
    public static class DataManager
    {
        public static Dictionary<Vector2, FixtureResponse> storedFixtures = new();

        public static async Task GetData()
        {
            int torneoID = Info.aperturaPrimeraID;
            int ano = 2012;

            string direction = AppDomain.CurrentDomain.BaseDirectory + $"Data/Fixtures";
            if (!Directory.Exists(direction))
            {
                Directory.CreateDirectory(direction);
            }

            for (int i = ano; i < 2024; i++)
            {
                direction = AppDomain.CurrentDomain.BaseDirectory + $"Data/Fixtures/Fixture {torneoID} {i}.json";
                if (!File.Exists(direction))
                {
                    using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?league={torneoID}&season={i}"))
                    {
                        response.EnsureSuccessStatusCode();
                        await Info.WriteResponseToJson(response, $"Data/Fixtures/Fixture {torneoID} {i}");
                    }
                }
                string file = File.ReadAllText(direction);
                var savedResponse = JsonConvert.DeserializeObject<FixtureResponse>(file);
                storedFixtures.Add(new Vector2(torneoID, i), savedResponse);
                if (torneoID == Info.aperturaPrimeraID)
                {
                    torneoID = Info.clausuraPrimeraID;
                    i--;
                }
                else
                {
                    torneoID = Info.aperturaPrimeraID;
                }
            }
        }
    }
}
