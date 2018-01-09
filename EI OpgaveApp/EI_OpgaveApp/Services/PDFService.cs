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
    public class PDFService
    {
        string endPoint = "api/asset/";

        public async Task<string> GetPDF(string id)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint + id);

                string result = response.Content.ReadAsStringAsync().Result;

                //var rootObject = JsonConvert.DeserializeObject<MaintenanceTask[]>(statsJson);

                return result;
            }
            catch
            {
                string jl = null;
                return jl;
            }
        }

        public async Task<PictureModel> PostPicture(PictureModel pic)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(pic);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                string url = "Api/MaintenanceTask/Picture/" + pic.TaskGuid;

                var response = await client.PostAsync(url, content);

                return JsonConvert.DeserializeObject<PictureModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                PictureModel jl = null;
                return jl;
            }
        }

        public async Task<PictureModel> PostPictureJobReg(PictureModel pic, string id)
        {
            try
            {
                pic.id = "{" + pic.id + "}";
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(pic);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Api/JobRegLine/Picture/" + pic.id, content);

                return JsonConvert.DeserializeObject<PictureModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                PictureModel jl = null;
                return jl;
            }
        }

        public async Task<AppErrorPicture> PostPictureAppError(AppErrorPicture pic, Guid guid)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var data = JsonConvert.SerializeObject(pic);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Api/AppError/Picture/" + pic.AppErrorGuid.ToString(), content);

                return JsonConvert.DeserializeObject<AppErrorPicture>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                AppErrorPicture jl = null;
                return jl;
            }
        }

    }
}
