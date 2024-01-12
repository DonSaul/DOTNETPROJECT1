using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class HttpConnector
    {
        private readonly HttpClient _httpClient;
        public HttpConnector() 
        { 
            _httpClient = new HttpClient();
        }
        public async Task<string> Get(string apiUrl)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) 
                { 
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception: " +ex.Message);
                return null;
            }
        }
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
