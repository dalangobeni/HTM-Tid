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
    public class WorkTypeService
    {
        string endPoint = "api/worktype/";
        public async Task<WorkType[]> GetWorkTypesAsync()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<WorkType[]>(statsJson);

                return rootObject;
            }
            catch
            {
                WorkType[] ml = null;
                return ml;
            }
        }
        public async Task<WorkType> GetWorkTypeAsync(string code)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint + code);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<WorkType>(statsJson);

                return rootObject;
            }
            catch
            {
                WorkType ml = null;
                return ml;
            }
        }
    }
}
