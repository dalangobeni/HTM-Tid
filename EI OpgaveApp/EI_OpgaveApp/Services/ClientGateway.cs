using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Services
{
    public class ClientGateway
    {
        string baseAddress;
        static HttpClient _client;
        static ClientGateway instance;
        private ClientGateway()
        {
            baseAddress = App.Database.GetConnectionSetting(0).Result.BaseAddress;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.BaseAddress = new Uri(baseAddress);
        }

        public static HttpClient GetHttpClient
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientGateway();
                }
                return _client;
            }
            private set { }
        }
    }
}
