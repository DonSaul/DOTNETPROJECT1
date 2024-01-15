using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly HttpClient _client;
        private readonly IServiceProvider _serviceProvider;

        public TechnicianService(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider)
        {
            _client = httpClientFactory.CreateClient("apiClient");
            _serviceProvider = serviceProvider;
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
        public async Task<TechnicianDetails[]> GetTechnicianByNameAsync(string technicianName)
        {
            /*
            Implementación de búsqueda de technician por nombre 

            Tareas: 
                 1. [ x ] Implementar TechnicianController
                 2. [ x ] Agregar función a ITechnicianService
                 3. [ x ] Implementar Technicians y Technician by ID en Controllers
                 4. [ x ] Preguntar a Hector por los Technicians con el mismo nombre
                 5. [ x ] Conseguir las WorkOrders del technician
                 6. [ x ] Dar formato a la respuesta en base a lo solicitado
                 7. [   ] Preguntar por el idioma en las respuestas, probablemente Inglés
                 8. [   ] Implementación de casos
                 9. [   ] lunes 15-01-2023: Merge con main-desarrollo
                10. [   ] Investigar posibles optimizaciones
                    

            Caso 1: Búsqueda de persona cuyo nombre es único en los registros
                Retornar a la persona en conjunto con sus work Orders
                - Testing:
                    [ x ] Como la respuesta ya provee a técnicos con nombres únicos, basta con testear desde la Interfaz Swagger  
            

            Caso 2: Dos personas con el mismo nombre
                Retornar a todos los técnicos que compartan el mismo nombre, junto con sus workOrders
                - Testing: 
                    [ x ] Implementación de array con Ténicos que compartan el mismo nombre

            Caso 3: Nombre no encontrado
                Tenemos como alternativas retornar el arreglo vacío o un mensaje de nombre no encontrado 
                - Testing: 
                    [  ] Probar con un string aleatorio, por ejemplo "asdf asdf"

            Caso 4: Fallo de conexión durante la solicitud (Sprint 3)
                - Testing:
                    [  ]

            */
            Technician[] technicians = await GetTechniciansAsync();
            var workOrderService = _serviceProvider.GetService(typeof(IWorkOrderService)) as IWorkOrderService;
            WorkOrder[] workOrders = await workOrderService.GetWorkOrdersAsync();
            /*
                Test 1: Technicians con el mismo nombre 

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
            */


            // Obtener la lista de todos los técnicos y WorkOrders
          
            // Filtrar los técnicos por el nombre usando LINQ
            // FIX: Hacer TechnicianDetails Una clase Heredada
            var filteredTechnicians = technicians
                .Where(t => t.Name.Equals(technicianName, StringComparison.OrdinalIgnoreCase))
                .Select(tech => new TechnicianDetails
                {
                    TechnicianId = tech.TechnicianId,
                    Technician = tech.Name,
                    Address = tech.Address,
                    // Anidar las WorkOrders correspondientes
                    WorkOrders = workOrders.Where(wo => wo.TechnicianId == tech.TechnicianId).ToArray()
                })
                .ToArray();

            return filteredTechnicians;

        }
    }

}
