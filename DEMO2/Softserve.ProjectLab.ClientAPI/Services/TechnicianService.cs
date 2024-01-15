﻿using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApiConnector _apiConnector;

        public TechnicianService(IServiceProvider serviceProvider, ApiConnector apiConnector)
        {
            _serviceProvider = serviceProvider;
            _apiConnector = apiConnector;
        }
        public async Task<Technician[]> GetTechniciansAsync()
        {
            return await _apiConnector.GetAsync<Technician[]>(ApiUrls.GetAllTechnicians);
        }
        public async Task<Technician> GetTechnicianAsync(int technicianId)
        {

            Technician technician = await _apiConnector.GetAsync<Technician>(ApiUrls.GetTechnicianById + technicianId);

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
                 9. [ x ] lunes 15-01-2023: Merge con main-desarrollo
                10. [ x ] Investigar posibles optimizaciones
                    

             Caso 1: Búsqueda de persona cuyo nombre es único en los registros
                Retornar a la persona en conjunto con sus work Orders
                - Testing:
                    [ x ] Testear desde la Interfaz Swagger  
            

            Caso 2: Dos personas con el mismo nombre
                Retornar a todos los técnicos que compartan el mismo nombre, junto con sus workOrders
                - Testing: 
                    [ x ] Implementación de array con Ténicos que compartan el mismo nombre

            Caso 3: Nombre no encontrado
                Tenemos como alternativas retornar el arreglo vacío o un mensaje de nombre no encontrado 
                - Testing: 
                    [ x ] Probar con un string aleatorio, por ejemplo "asdf asdf"

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
            var filteredTechnicians = technicians
                .Where(t => t.Name.Equals(technicianName, StringComparison.OrdinalIgnoreCase))
                .Select(tech => new TechnicianDetails
                {
                    TechnicianId = tech.TechnicianId,
                    Technician = tech.Name,
                    Address = tech.Address,
                    
                    // Anidar las WorkOrders correspondientes
                    WorkOrders = workOrders
                        .Where(wo => wo.TechnicianId == tech.TechnicianId)
                        .ToArray()
                })
                .ToArray();

            return filteredTechnicians;

        }
    }

}
