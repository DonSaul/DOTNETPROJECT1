using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class StatusService : IStatusService
    {
        private readonly HttpClient _client;

        public StatusService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("apiClient");
        }


        public async Task<Status[]> GetStatusesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/Status");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error al obtener los estados");
            }

            string body = await response.Content.ReadAsStringAsync();
            Status[] statuses = JsonConvert.DeserializeObject<Status[]>(body);

            return statuses;
        }
    }

}
