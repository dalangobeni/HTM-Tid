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
    public class MaintenanceActivityService
    {
        string endPoint = "api/TaskActivities/";

        public async Task<MaintenanceActivity[]> GetMaintenanceActivitiesAsync()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<MaintenanceActivity[]>(statsJson);

                return rootObject;
            }
            catch
            {
                MaintenanceActivity[] jl = null;
                return jl;
            }
        }

        public async Task<MaintenanceActivity> UpdateTask(MaintenanceActivity task)
        {
            try
            {
                string newendPoint = null;
                newendPoint = endPoint + "update";

                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(task);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(newendPoint, content);

                return JsonConvert.DeserializeObject<MaintenanceActivity>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                MaintenanceActivity jl = null;
                return jl;
            }
        }
    }
}
