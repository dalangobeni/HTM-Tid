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
    public class ResourcesService
    {
        string endPoint = "api/Resources/";

        public async Task<Resources[]> GetResourcesAsync()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<Resources[]>(statsJson);

                return rootObject;
            }
            catch
            {
                Resources[] ml = null;
                return ml;
            }
        }
        public async Task<Resources> GetResourceAsync(string no)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint + no);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<Resources>(statsJson);

                return rootObject;
            }
            catch
            {
                Resources ml = null;
                return ml;
            }
        }

    }
}
