﻿using Newtonsoft.Json;
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
            // test
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
        public async Task<Technician[]> GetTechnicianByNameAsync(string technicianName)
        {
            // Tareas: 
            // [ x ] Implementar TechnicianController
            // [ x ] retornar Technicians y Technician by ID

            // Implementar búsqueda de technician por nombre 
            // Dos personas con el mismo nombre?
            // opción 1: retornar todos los técnicos con el mismo nombre
            // Conseguir las WorkOrders del technician


            throw new NotImplementedException("La búsqueda de técnicos por nombre aún no está implementada.");

        }
    }

}
