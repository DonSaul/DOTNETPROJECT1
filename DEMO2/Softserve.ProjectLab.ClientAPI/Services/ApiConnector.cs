using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class ApiConnector : IApiConnector
    {
        private readonly HttpClient _client;
        private readonly int _maxRetries;
        private readonly TimeSpan _delay;

        public ApiConnector(IHttpClientFactory httpClientFactory, int maxRetries = 3, TimeSpan? delay = null)
        {
            _client = httpClientFactory.CreateClient("apiClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _maxRetries = maxRetries;
            _delay = delay ?? TimeSpan.FromSeconds(3);
        }

        public virtual async Task<T> GetAsync<T>(string endpoint)
        {
            for (int count = 0; count < _maxRetries; count++)
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync(endpoint);
                    await HandleResponseAsync(response);

                    string body = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(body);
                }
                catch (HttpIOException)
                {
                    return default(T);
                }
                catch (Exception ex)
                {
                    if (count == (_maxRetries - 1))
                    {
                        throw new ArgumentNullException(ex.Message);
                    }

                    await Task.Delay(_delay);
                }
            }
            throw new InvalidOperationException("Retry has reached an unexpected state. Please consult your IT department.");
        }

        public async Task HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new HttpIOException(HttpRequestError.ResponseEnded);
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}
