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
        private readonly IApiConnector _apiConnector;

        public TechnicianService(IServiceProvider serviceProvider, IApiConnector apiConnector)
        {
            _serviceProvider = serviceProvider;
            _apiConnector = apiConnector ?? throw new ArgumentNullException(nameof(apiConnector));
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
             
                var searchTerms = technicianName.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var filteredTechnicians = technicians
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
                    WorkOrders = workOrders
                        .Where(wo => wo.TechnicianId == tech.TechnicianId)
                        .Select(w => new WorkOrderDetails
                        {
                            WorkOrderName = w.WorkOrderName,
                            Technician = tech.Name,
                            WorkType = workTypes.Where(wt => wt.Id == w.WorkTypeId).First().Name,
							Status = statuses.Where(s => s.Id == w.StatusId).First().Name,
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
