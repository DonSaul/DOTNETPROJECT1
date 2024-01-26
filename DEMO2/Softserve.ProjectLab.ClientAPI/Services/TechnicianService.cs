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
        public async Task<List<TechnicianDetails> > GetTechnicianByNameAsync(string technicianName)
        {
            /*
            Implementation of technician search by name
            
            Case 1: Basic Search
                - Example: "Mauricio Sepulveda"
                - Results: Returns Technician Details for "Mauricio Sepulveda".

            Case 2: Two people with the same name
                - Example: "mauricio"
                - Result: Returns a list of all people with a name similar to "Mauricio"

            Case 3: Name not found
                - Example: "1234"
                - Result: Returns an Empty List

            Case 4: Capitalization
                - Ex: "mauricio sepulveda"
                - Result: Returns "Mauricio Sepulveda", to handle case-insensitivity.

            Case 5: Accent Marks
                - Ex: "Natalia Henriquez"
                - Result: "Natalia Henríquez"

            Case 6: Inversed Accent Marks
                - Ex: "Natalia Henríquez"
                - Result: "Natalia Henriquez" (In case the name was saved without accent mark)

            Case 7: Special Characters
                - Ex: "O'Higgins", "François", "Jürgen"
                - Result: Technicians with special characters on their names
                - How much to cover(?) (Could be English Names)

            Case 8: Empty Search
                - Ex: ""
                - Result: []

            Case 9: Typing Errors
                - Ex: "gabeel" instead of "gabriel" and "gabiel" instead of "gabriel"
                    - Result: When the closer match is gabriel, should return an Empty List
            
            Case 10: Unorderded Names
                - Ex: "Sepulveda Mauricio"
                - Returns the profile of "Mauricio Sepulveda", showing the system's ability to handle name components in any order.

            Case 11: Partial Name Search
                - Ex: "mau"
                - Result: Returns "Mauricio Sepulveda" and other similar names, demonstrating partial match capabilities.

            Case 12: Partial Name and Last Name Search
                - Ex: "mau sep"
                - Result:Result: Returns "Mauricio Sepulveda" and other similar names, demonstrating partial match capabilities.

            Case 13: Error Connection
                - Ex: When the connection to Docker is turned Off
                - Result: Error response 400
            */
            




            /* Handles NUll, Empty string ("") and WhiteSpaces, returning empty array */
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
                         new Technician { Name = "Diego Ardiles", TechnicianId = 6, Address = "Calle Blanco 7259, Santiago" },
                     };
                */


                var normalizedSearchTerms = RemoveDiacritics(technicianName).ToLower().Split(' ');


                // Filter technicians by name using LINQ
                var filteredTechnicians = technicians
                    .Where(t => normalizedSearchTerms.All(term => RemoveDiacritics(t.Name).ToLower().Contains(term)))
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

        /// <summary>
        /// Removes diacritics (like accents) from a string.
        /// </summary>
        /// <param name="text">The string from which to remove diacritics.</param>
        /// <returns>A string without diacritics.</returns>
        /// <example>
        /// Here are some examples demonstrating how the function works with different inputs:
        ///    Input: "Françoise"  Output: "Francoise"
        ///    Input: "Åke"        Output: "Ake"
        ///    Input: "München"    Output: "Munchen"
        /// </example>
        private static string RemoveDiacritics(string text)
        {
            // Normalizes the string into the canonical decomposition form,
            // where diacritics are separated from their base characters.
            var normalizedString = text.Normalize(NormalizationForm.FormD);

            // StringBuilder is used to construct the new string without diacritics.
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                // Gets the Unicode category of the character.
                // This helps to identify if it's a diacritic.
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                // If the character is not a diacritic (non-spacing mark),
                // it is added to the StringBuilder.
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

}
