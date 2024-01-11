using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly HttpClient _client;

        public TechnicianService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("apiClient");
        }

        public async Task<Technician[]> GetTechniciansAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/Technician");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error al obtener los técnicos");
            }

            string body = await response.Content.ReadAsStringAsync();
            Technician[] technicians = JsonConvert.DeserializeObject<Technician[]>(body);

            return technicians;
        }
        public async Task<Technician> GetTechnicianAsync(int technicianId)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/Technician/{technicianId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Error al obtener el técnico con el ID: {technicianId}");
            }

            string body = await response.Content.ReadAsStringAsync();
            Technician technician = JsonConvert.DeserializeObject<Technician>(body);

            return technician;
        }

    }

}
