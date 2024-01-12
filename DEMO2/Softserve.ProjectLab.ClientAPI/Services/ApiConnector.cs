using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class ApiConnector
    {
        private readonly HttpClient _client;

        public ApiConnector(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("apiClient");
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            await HandleResponseAsync(response);

            string body = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(body);
        }

        private async Task HandleResponseAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}
