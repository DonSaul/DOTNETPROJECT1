using Newtonsoft.Json;
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
            try
            {
                return await _apiConnector.GetAsync<Technician[]>(ApiUrls.GetAllTechnicians);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with GetTechnicians: {ex.Message}");
                throw;
            }
        }
        public async Task<Technician> GetTechnicianAsync(int technicianId)
        {
            try
            {
                return await _apiConnector.GetAsync<Technician>(ApiUrls.GetTechnicianById + technicianId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with GetTechnicianByID: {ex.Message}");
                throw;
            }
        }
        public async Task<List<TechnicianDetails> > GetTechnicianByNameAsync(string technicianName)
        {
            /*
            Implementation of technician search by name

            Tasks:
                1. [ x ] Implement TechnicianController
                2. [ x ] Add function to ITechnicianService
                3. [ x ] Implement Technicians and Technician by ID in Controllers
                4. [ x ] Ask Hector about Technicians with the same name
                5. [ x ] Retrieve WorkOrders of the technician

                6. [ x ] Format the response according to the request
                7. [ x ] Ask about the language for responses, probably English

                8. [   ] Implementation of cases                    

             Case 1: Search for a person whose name is unique in the records
                Return the person along with their work orders
                - Testing:
                    [ x ] Test from Swagger Interface 
            

            Caso 2: Two people with the same name
                Return all technicians who share the same name, along with their work orders
                - Testing: 
                    [ x ] Implementation of an array with Technicians sharing the same name

            Caso 3: Name not found
                Return an empty array
                - Testing: 
                    [ x ] Test with a random string, for example "Lorem Ipsum"

            Caso 4: Connection failure during request (Sprint 3)
                - Testing:
                    [  ]
            */
            //Technician[] technicians = await GetTechniciansAsync();
            try
            {
                var workOrderService = _serviceProvider.GetService(typeof(IWorkOrderService)) as IWorkOrderService;
                var statusService = _serviceProvider.GetService(typeof(IStatusService)) as IStatusService;
                var workTypeService = _serviceProvider.GetService(typeof(IWorkTypeService)) as IWorkTypeService;


                var workOrdersTask = workOrderService.GetWorkOrdersAsync();
                var techniciansTask = GetTechniciansAsync();
                var statusesTask = statusService.GetStatusesAsync();
                var workTypesTask = workTypeService.GetWorkTypesAsync();

                await Task.WhenAll(workOrdersTask, statusesTask, techniciansTask, workTypesTask);

                var workOrders = workOrdersTask.Result;
                var technicians = techniciansTask.Result;
                var statuses = statusesTask.Result;
                var workTypes = workTypesTask.Result;


                //   Test 1: Technicians with the same Name
                /*
                     Technician[] testTechnicians = new Technician[]
                     {
                         new Technician { Name = "Mauricio Sepulveda", TechnicianId = 1, Address = "Carlos Condell 5806, Valparaiso" },
                         new Technician { Name = "Natalia Henriquez", TechnicianId = 2, Address = "Diego Portales 3666, Valparaiso" },
                         new Technician { Name = "Natalia Henriquez", TechnicianId = 3, Address = "Arturo Prat 1861, Santiago" },
                         new Technician { Name = "Ramon Sepulveda", TechnicianId = 4, Address = "Av. Matucana 9075, Santiago" },
                         new Technician { Name = "Gabriel Rivas", TechnicianId = 5, Address = "Aníbal Pinto 578, Valparaiso" },
                         new Technician { Name = "Diego Ardiles", TechnicianId = 6, Address = "Calle Blanco 7259, Santiago" },
                     };
                */

                // Get the list of all technicians and WorkOrders

                // Filter technicians by name using LINQ
                var filteredTechnicians = technicians
                    .Where(t => t.Name.IndexOf(technicianName, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(tech => new TechnicianDetails
                    {
                        TechnicianId = tech.TechnicianId,
                        Technician = tech.Name,
                        Address = tech.Address,

                        // Nest the corresponding WorkOrders
                        WorkOrders = workOrders
                                      .Where(wo => wo.TechnicianId == tech.TechnicianId)
                                      .Select(w => new WorkOrderDetails
                                      {
                                          WorkOrderName = w.WorkOrderName,
                                          Technician = tech.Name,
                                          WorkType = workTypes.Where(wt => wt.Id == w.WorkTypeId).First().Name,
                                          //     Status =        statuses.Where(s => s.Id == w.StatusId).First().Name,
                                          EndTime = w.EndTime.HasValue ? (DateTimeOffset)w.EndTime.Value : (DateTimeOffset?)null,
                                          StartTime = w.StartTime.HasValue ? (DateTimeOffset)w.StartTime.Value : (DateTimeOffset?)null
                                      }).ToArray()
                    }).ToList();

                return filteredTechnicians;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                throw;
            }
        }
    }

}
