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
    public class AppErrorService
    {
        string endPoint = "api/AppError/";
        public async Task<AppError[]> GetAppErrors()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<AppError[]>(statsJson);

                return rootObject;
            }
            catch
            {
                AppError[] jl = null;
                return jl;
            }
        }

        public async Task<AppError> CreateAppError(AppError task)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(task);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endPoint + "create", content);

                return JsonConvert.DeserializeObject<AppError>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                AppError jl = null;
                return jl;
            }
        }
    }
}
