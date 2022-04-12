using Sinatra.ApiClient.Clients;
using System;
using System.Net.Http;

namespace Sinatra.ApiClient
{
    public class SinatraApiClient
    {
        private static SinatraApiClient ApiClient;

        public static SinatraApiClient GetInstance(string baseAddress)
        {
            if (ApiClient == null)
            {
                ApiClient = new SinatraApiClient(baseAddress);
            }

            return ApiClient;
        }

        private SinatraApiClient(string baseAddress)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            var simpleClient = new SimpleClient(httpClient);

            Users = new UsersApiClient(simpleClient);
        }

        public UsersApiClient Users { get; }
    }
}
