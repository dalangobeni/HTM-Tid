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
    public class TimeRegistrationService
    {
        string endPoint = "api/meetleave/";

        public async Task<TimeRegistrationModel[]> GetTimeRegsAsync()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<TimeRegistrationModel[]>(statsJson);

                return rootObject;
            }
            catch
            {
                TimeRegistrationModel[] jl = null;
                return jl;
            }
        }

        public async Task<TimeRegistrationModel> UpdateTimeReg(TimeRegistrationModel timeReg)
        {
            try
            {
                string newendPoint = null;
                newendPoint = endPoint + "update/" + timeReg.No;
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(timeReg);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(newendPoint, content);

                return JsonConvert.DeserializeObject<TimeRegistrationModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                TimeRegistrationModel jl = null;
                return jl;
            }
        }

        public async Task<TimeRegistrationModel> CreateTimeReg(TimeRegistrationModel timeReg)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(timeReg);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endPoint + "create", content);

                return JsonConvert.DeserializeObject<TimeRegistrationModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                TimeRegistrationModel jl = null;
                return jl;
            }
        }
    }
}
