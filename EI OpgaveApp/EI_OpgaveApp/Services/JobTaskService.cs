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
    public class JobTaskService
    {
        string endPoint = "api/JobTasks/";

        public async Task<JobTask[]> GetJobTasksAsync()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<JobTask[]>(statsJson);

                return rootObject;
            }
            catch
            {
                JobTask[] jl = null;
                return jl;
            }
        }
    }
}
