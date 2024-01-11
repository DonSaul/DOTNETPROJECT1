using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkTypeService : IWorkTypeService
    {
        private readonly HttpClient _client;

        public WorkTypeService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("apiClient");
        }

        public async Task<WorkType[]> GetWorkTypesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/WorkType");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error al obtener los tipos de trabajo");
            }

            string body = await response.Content.ReadAsStringAsync();
            WorkType[] workTypes = JsonConvert.DeserializeObject<WorkType[]>(body);

            return workTypes;
        }
    }

}
