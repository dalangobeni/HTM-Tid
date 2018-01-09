using EI_OpgaveApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Services
{
    public class ResourcePersonService
    {
        string endPoint = "api/resourcePerson/";
        public async Task<ResourcePerson[]> GetResroucePersons()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<ResourcePerson[]>(statsJson);

                return rootObject;
            }
            catch
            {
                ResourcePerson[] ml = null;
                return ml;
            }
        }
        public async Task<ResourcePerson> GetResroucePerson(string no)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint + no);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<ResourcePerson>(statsJson);

                return rootObject;
            }
            catch
            {
                ResourcePerson ml = null;
                return ml;
            }
        }
    }
}
