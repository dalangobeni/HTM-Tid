using EI_OpgaveApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Services
{
    public class TimeRegStatsService
    {
        string endPoint = "api/timeregstats/";
        public async Task<TimeRegStat> GetTimeRegStatsAsync(string resourceCode)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint + resourceCode);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<TimeRegStat>(statsJson);

                return rootObject;
            }
            catch
            {
                TimeRegStat ml = new TimeRegStat();
                ml.User = "";
                return ml;
            }
        }
    }
}
