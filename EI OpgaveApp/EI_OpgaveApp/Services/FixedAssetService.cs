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
    public class FixedAssetService
    {
        string endPoint = "api/FixedAsset/";
        public async Task<FixedAsset[]> GetFixedAssets()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync(endPoint);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<FixedAsset[]>(statsJson);

                return rootObject;
            }
            catch
            {
                FixedAsset[] jl = null;
                return jl;
            }
        }
    }
}
