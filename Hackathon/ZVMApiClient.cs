using CanIFailover.JSONObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CanIFailover
{
    public class ZVMApiClient
    {
        public ZVMApiClient (string username, string password)
        {
            this.username = username;
            this.password = password;
            
        }
        private HttpClient httpClient { get; set; }
        private string baseUri { get; set; } = "https://localhost:9669";
        private string username { get; set; }
        private string password { get; set; }


        public async Task<bool> AddTokenToHttpClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // set TLS1.2
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => // ignore cert errors
            {
                return true;
            };
            httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(baseUri) }; 
            byte[] byteArray = Encoding.ASCII.GetBytes(username + ":" + password); 
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await httpClient.PostAsync(baseUri + "/v1/session/add", null);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Failed to init session");
                Console.WriteLine("Status code is " + response.StatusCode);
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorMessage);
                return false;
                //throw new Exception("Failed to init session");

            }
            string x_zerto_session = response.Headers.GetValues("x-zerto-session").FirstOrDefault();
            httpClient.DefaultRequestHeaders.Add("x-zerto-session", x_zerto_session);
            return true;
        }
        public async Task<List<ZertoHost>> GetZertoHosts(string SiteId)
        {
            HttpResponseMessage response = await httpClient.GetAsync(baseUri + ZertoHost.Uri(SiteId));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorMessage);
                throw new Exception("Failed to init session");
            }
            string content = await response.Content.ReadAsStringAsync();
            List<ZertoHost> returnList = JsonConvert.DeserializeObject<List<ZertoHost>>(content);
            return returnList;
        }
        public async Task<string> GetSiteId()
        {
            HttpResponseMessage response = await httpClient.GetAsync(baseUri + LocalSiteObject.Uri);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorMessage);
                throw new Exception("Failed to init session");
            }
            string content = await response.Content.ReadAsStringAsync();
            LocalSiteObject localSiteObject = JsonConvert.DeserializeObject<LocalSiteObject>(content);
            return localSiteObject.SiteIdentifier;
        }
        //private async Task<>
    }
}
