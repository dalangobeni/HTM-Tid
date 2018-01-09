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
    public class JobRecLineService
    {
        string endPoint = "api/TimeRegistration/";
        public async Task<JobRecLine[]> GetJobRecLines()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<JobRecLine[]>(statsJson);

                return rootObject;
            }
            catch
            {
                JobRecLine[] jl = null;
                return jl;
            }
        }

        public async Task<JobRecLine> UpdateJobRecLine(JobRecLine task)
        {
            try
            {
                string newendPoint = null;
                newendPoint = endPoint + "update/";
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(task);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(newendPoint, content);

                return JsonConvert.DeserializeObject<JobRecLine>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                JobRecLine jl = null;
                return jl;
            }
        }

        public async Task<bool> CreateJobRecLine(JobRecLine task)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(task);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endPoint + "create", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
