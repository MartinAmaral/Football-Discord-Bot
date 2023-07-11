using Newtonsoft.Json;

namespace Discord_Bot
{
    public static class Info
    {
        public const int aperturaPrimeraID = 268;
        public const int clausuraPrimeraID = 270;

        public const int anoBase = 2012;

        public const int sudamericaID = 2347;
        public const int penarolID = 2348;
        public const int elTanqueSisleyID = 2349;
        public const int defensorSportingID = 2350;
        public const int riverPlateID = 2351;
        public const int danubioID = 2352;
        public const int juventudID = 2353;
        public const int ramplaJuniosID = 2354;
        public const int plazaColoniaID = 2355;
        public const int nacionalID = 2356;
        public const int fenixID = 2357;
        public const int liverpoolID = 2358;
        public const int racingID = 2359;
        public const int wanderersID = 2360;
        public const int bostonRiverID = 2361;
        public const int cerroID = 2362;
        public const int progresoID = 2363;
        public const int atenasID = 2364;
        public const int torqueID = 2365;
        public const int villaTeresaID = 2366;
        public const int cerritoID = 2367;
        public const int centralEspanolID = 2368;
        public const int cerroLargoID = 2369;
        public const int deportivoMaldonadoID = 2370;
        public const int rentistasID = 2371;
        public const int villaEspanolaID = 2372;
        public const int miramarID = 2373;
        public const int tacuaremboID = 2374;
        public const int albionID = 2378;
        public const int laLuzID = 18295;

        public static bool IsValidTeam(string team, out int teamID, out string teamCleanName)
        {
            team = team.ToLower().RemoveSpanish();
            if (team.Contains("penarol"))
            {
                teamID = penarolID;
                teamCleanName = "Peñarol";
                return true;
            }
            else if (team.Contains("defensor"))
            {
                teamID = defensorSportingID;
                teamCleanName = "Defensor";
                return true;
            }
            else if (team.Contains("nacional"))
            {
                teamID = nacionalID;
                teamCleanName = "Nacional";
                return true;
            }
            else if (team.Contains("boston") || team.Contains("river") && team.Contains("boston"))
            {
                teamID = bostonRiverID;
                teamCleanName = "Boston River";
                return true;
            }
            else if (team.Contains("river"))
            {
                teamID = riverPlateID;
                teamCleanName = "River Plater";
                return true;
            }
            else if (team.Contains("cerro largo"))
            {
                teamID = cerroLargoID;
                teamCleanName = "Cerro Largo";
                return true;
            }
            else if (team.Contains("cerro"))
            {
                teamID = cerroID;
                teamCleanName = "Cerro";
                return true;
            }
            else if (team.Contains("danubio"))
            {
                teamID = danubioID;
                teamCleanName = "Danubio";
                return true;
            }
            else if (team.Contains("colonia") || team.Contains("plaza"))
            {
                teamID = plazaColoniaID;
                teamCleanName = "Plaza Colonia";
                return true;
            }
            else if (team.Contains("fenix"))
            {
                teamID = fenixID;
                teamCleanName = "Fénix";
                return true;
            }
            else if (team.Contains("liverpool"))
            {
                teamID = liverpoolID;
                teamCleanName = "Liverpool";
                return true;
            }
            else if (team.Contains("racing"))
            {
                teamID = racingID;
                teamCleanName = "Racing";
                return true;
            }
            else if (team.Contains("wanderers"))
            {
                teamID = wanderersID;
                teamCleanName = "Wanderers";
                return true;
            }
            else if (team.Contains("torque"))
            {
                teamID = torqueID;
                teamCleanName = "Torque";
                return true;
            }
            else if (team.Contains("maldonado"))
            {
                teamID = deportivoMaldonadoID;
                teamCleanName = "Deportivo Maldonaldo";
                return true;
            }
            else if (team.Contains("luz"))
            {
                teamID = laLuzID;
                teamCleanName = "La Luz";
                return true;
            }
            else if (team.Contains("juventud"))
            {
                teamID = juventudID;
                teamCleanName = "Juventud";
                return true;

            }
            else if (team.Contains("rentistas"))
            {
                teamID = rentistasID;
                teamCleanName = "Rentistas";
                return true;

            }
            else if (team.Contains("sudamerica") || team.Contains("sud") || team.Contains("america"))
            {
                teamID = sudamericaID;
                teamCleanName = "SUD America";
                return true;

            }
            else if (team.Contains("sisley") || team.Contains("tanque"))
            {
                teamID = elTanqueSisleyID;
                teamCleanName = "El Tanque Sisley";
                return true;

            }
            else if (team.Contains("miramar"))
            {
                teamID = miramarID;
                teamCleanName = "Miramar";
                return true;

            }
            else if (team.Contains("tacuarembo"))
            {
                teamID = tacuaremboID;
                teamCleanName = "Tacuarembo";
                return true;

            }
            else if (team.Contains("atenas"))
            {
                teamID = atenasID;
                teamCleanName = "Atenas";
                return true;

            }
            else if (team.Contains("rampla"))
            {
                teamID = ramplaJuniosID;
                teamCleanName = "Rampla Juniors";
                return true;

            }
            else if (team.Contains("teresa"))
            {
                teamID = villaTeresaID;
                teamCleanName = "Villa Teresa";
                return true;
            }
            else if (team.Contains("espanola"))
            {
                teamID = villaEspanolaID;
                teamCleanName = "Villa Española";
                return true;
            }
            else if (team.Contains("progreso"))
            {
                teamID = progresoID;
                teamCleanName = "Progreso";
                return true;
            }
            else if (team.Contains("cerrito"))
            {
                teamID = cerritoID;
                teamCleanName = "Cerrito";
                return true;
            }
            else if (team.Contains("albion"))
            {
                teamID = albionID;
                teamCleanName = "Albion";
                return true;
            }
            else if (team.Contains("espanol"))
            {
                teamID = centralEspanolID;
                teamCleanName = "Central español";
                return true;
            }

            //"id": 5559,
            //"name": "Racing",

            teamID = 0;
            teamCleanName = "No existe";
            return false;

        }

        public static string GetTorneoName(int id)
        {
            switch (id)
            {
                case aperturaPrimeraID:
                    return "Apertura";
                case clausuraPrimeraID:
                    return "Clausura";
            }
            return "Torneo invalido";
        }
        public static async Task WriteResponseToJson(HttpResponseMessage response, string fileName = "test")
        {
            var resposeBody2 = await response.Content.ReadAsStringAsync();
            var finalrespose = JsonConvert.DeserializeObject(resposeBody2);
            using var writer = new StreamWriter(fileName += ".json");
            writer.Write(finalrespose);
        }

        public static void ConsoleJson(object data, bool save = false, string fileName = "default")
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            if (save)
            {
                using var writer = new StreamWriter($"{fileName}{DateTime.Now}.json");
                writer.Write(json);
            }
            Console.WriteLine(json);
        }
    }
}
