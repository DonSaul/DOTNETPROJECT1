using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Globalization;
using System.Net;
using System.Text;

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

        /// <summary>
        /// Searches for technicians by name and returns a list of matching technicians.
        /// </summary>
        /// <param name="technicianName">The name of the technician to search for.</param>
        /// <returns>A list of TechnicianDetails objects matching the search criteria.</returns>
        /// <example>
        /// Here are some examples demonstrating how the function works with different inputs:
        ///    Input: "Mauricio Sepulveda"  Output: Details for "Mauricio Sepulveda"
        ///    Input: "mauricio"            Output: All technicians named "Mauricio"
        ///    Input: "  Arturo"            Output: Details for "Arturo" (handles extra spaces)
        ///    Input: "Sepulveda Mauricio"  Output: Empty list (unordered names not allowed)
        ///    Input: "mau"                 Output: Empty list (partial name searches not allowed)
        /// </example>
        /// <remarks>
        /// This function handles various search cases including full name search, first name only, last name only,
        /// case insensitivity, special characters, and exact matches while excluding partial name searches, 
        /// unordered names, and typing errors. It also handles extra spaces and returns an empty list for non-existent names.
        /// </remarks>
        public async Task<List<TechnicianDetails> > GetTechnicianByNameAsync(string technicianName)
        {
            /*
            Implementation of technician search by name
            
            Case 1: Basic Search
                - Example: "Mauricio Sepulveda"
                - Results: Returns Technician Details for "Mauricio Sepulveda".

            Case 2: Search by First Name
                - Example: "mauricio"
                - Result: Returns a list of all people with a name similar to "Mauricio"

            Case 3: Search by Last Name
                - Example: "Sepulveda"
                - Result: Returns a list of all people with Sepulveda on their Last names

            Case 4: Accent Marks Not present
                - Ex: "Natalia Henriquez"
                - Result:Must return Technician's with name "Natalia Henriquez". "Natalia Henríquez" will be excluded, 
                   due to Accent Difference to preserve exact results
            
            Case 5: Capitalization
                - Ex: "mauricio sepulveda"
                - Result: Returns "Mauricio Sepulveda", to handle case-insensitivity.

            Case 6: Unorderded Names
                - Ex: "Sepulveda Mauricio"
                - Method Not Allowed, so it should return an empty List 

            Case 7: Special Characters
                - Ex: "O'Higgins", "François", "Jürgen"
                - Result: Technicians with special characters on their names

            Case 8: Name not found
                - Example: "1234"
                - Result: Returns an Empty List

            Case 9: Empty Search, Null string or WhiteSpaces only
                - Ex: ""
                - Result: Returns Empty List

            Case 10: Typing Errors
                - Ex: "gabeel" instead of "gabriel" and "gabiel" instead of "gabriel"
                - Result: Empty List, not allowed

            Case 11: Extra Spaces
                - Ex: "  Arturo", " Mauricio   Sepulveda  "
                - Result: Search for terms on string, "Arturo" and "Mauricio Sepulveda" respectively

            Case 11: Partial Name Search
                - Ex: "mau"
                - Result: this method is not allowed, so it will return an Empty List,

            Case 12: Partial Name and Last Name Search
                - Ex: "mau sep"
                - Result: Not allowed method, so it will return an Empty List

            Case 13: Error Connection
                - Ex: When the connection to Docker is turned Off
                - Result: Error response 400
            */

            /* Handles NUll, Empty string ("") and WhiteSpaces, returning empty array */

            //technicianName = " Arturo";
            
            if (string.IsNullOrWhiteSpace(technicianName))
            {
                return new List<TechnicianDetails>();
            }

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
                         new Technician { Name = "Diego O'Higgins", TechnicianId = 6, Address = "Calle Blanco 7259, Santiago" },
                     };
                */




             
                var searchTerms = technicianName.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);


                var filteredTechnicians = 
                    technicians
                    //testTechnicians
                .Where(t => {
                    var techNameWords = t.Name.ToLowerInvariant().Split(' ');
                    int searchTermIndex = 0;
                    foreach (var word in techNameWords)
                    {
                        if (word.Equals(searchTerms[searchTermIndex]))
                        {
                            searchTermIndex++;
                            if (searchTermIndex == searchTerms.Length)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                })
                .Select(tech => new TechnicianDetails
                {
                    TechnicianId = tech.TechnicianId,
                    Technician = tech.Name,
                    Address = tech.Address,
                    // Añadir detalles adicionales según sea necesario
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

                // Filter technicians by name using LINQ
                /*
                var normalizedSearchTerms = technicianName.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var filteredTechnicians = technicians
                    .Where(t => searchTerms.All(term => t.Name.ToLower().Contains(term)))
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
                    */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                throw;
            }
        }

    }

}
