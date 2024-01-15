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
        public async Task<Technician[]> GetTechnicianByNameAsync(string technicianName)
        {
            /*
            Implementación de búsqueda de technician por nombre 

            Tareas: 
                 1. [ x ] Implementar TechnicianController
                 2. [ x ] Agregar función a ITechnicianService
                 3. [ x ] Implementar Technicians y Technician by ID en Controllers
                 4. [ x ] Preguntar a Hector por los Technicians con el mismo nombre
                 5. [   ] Conseguir las WorkOrders del technician
                 6. [   ] Dar formato a la respuesta en base a lo solicitado
                 7. [   ] Preguntar por el idioma en las respuestas, probablemente Inglés

            Caso 1: Búsqueda de persona cuyo nombre es único en los registros
                Retornar a la persona en conjunto con sus work Orders
                Testing:
                Como la respuesta ya provee a ténicos con nombres únicos, basta con testear desde la Interfaz Swagger  
                [ x ] Testear a Técnicos con nombres diferentes


            Caso 2: Dos personas con el mismo nombre
                Retornar a todos los técnicos que compartan el mismo nombre, junto con sus workOrders
                - Testing: 
                Implementación de array con Ténicos que compartan el mismo nombre
                [ x ] 

            Caso 3: Nombre no encontrado
                Retornar mensaje de nombre no encontrado
                - Testing: 
                [  ] Probar con un string aleatorio, por ejemplo "asdf asdf"

            Caso 4: Fallo de conexión durante la solicitud (Sprint 3) 
                - Testing:

            */
            Technician[] technicians = await GetTechniciansAsync();


            Technician[] testTechnicians = new Technician[]
            {
                new Technician { Name = "Mauricio Sepulveda", TechnicianId = 1, Address = "Carlos Condell 5806, Valparaiso" },
                new Technician { Name = "Natalia Henriquez", TechnicianId = 2, Address = "Diego Portales 3666, Valparaiso" },
                new Technician { Name = "Natalia Henriquez", TechnicianId = 3, Address = "Arturo Prat 1861, Santiago" },
                new Technician { Name = "Ramon Sepulveda", TechnicianId = 4, Address = "Av. Matucana 9075, Santiago" },
                new Technician { Name = "Gabriel Rivas", TechnicianId = 5, Address = "Aníbal Pinto 578, Valparaiso" },
                new Technician { Name = "Diego Ardiles", TechnicianId = 6, Address = "Calle Blanco 7259, Santiago" },
            };

            // Filtrar los técnicos por el nombre usando LINQ
            var filteredTestTechnicians = testTechnicians.Where(
                    t => t.Name.Equals(technicianName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return filteredTestTechnicians;

            // Filtrar los técnicos por el nombre usando LINQ
            var filteredTechnicians = technicians.Where(
                    t => t.Name.Equals(technicianName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return filteredTechnicians;

        }
    }

}
