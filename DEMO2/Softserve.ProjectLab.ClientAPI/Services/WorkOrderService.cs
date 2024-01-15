using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ApiConnector _apiConnector;
        private readonly IStatusService _statusService;
        private readonly ITechnicianService _technicianService;
        private readonly IWorkTypeService _workTypeService;
        public WorkOrderService(ApiConnector apiConnector, IHttpClientFactory httpClientFactory, IStatusService statusService, ITechnicianService technicianService, IWorkTypeService workTypeService)
        {
            _apiConnector = apiConnector;
            _statusService = statusService;
            _technicianService = technicianService;
            _workTypeService = workTypeService;
        }
        public async Task<WorkOrder[]> GetWorkOrdersAsync()
        {
            try
            {    
                return await _apiConnector.GetAsync<WorkOrder[]>(ApiUrls.GetAllWorkOrders);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error with GetAllWorkOrders: {ex.Message}");
                throw;
            }

        }
        public async Task<WorkOrder> GetWorkOrderAsync(string workOrderName)
        {
            try
            {
                return await _apiConnector.GetAsync<WorkOrder>(ApiUrls.GetWorkOrderByName + workOrderName);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error with GetWorkOrderByName: {ex.Message}");
                throw; 
            }    
        }

        public async Task<List<WorkOrderDetails>> GetWorkOrdersAsync(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status)
        {
            try
            {
                var workOrdersTask = GetWorkOrdersAsync();
                var techniciansTask = _technicianService.GetTechniciansAsync();
                var statusesTask = _statusService.GetStatusesAsync();
                var workTypesTask = _workTypeService.GetWorkTypesAsync();

                await Task.WhenAll(workOrdersTask, statusesTask, techniciansTask, workTypesTask);

                var workOrders = workOrdersTask.Result;
                var technicians = techniciansTask.Result;
                var statuses = statusesTask.Result;
                var workTypes = workTypesTask.Result;

                //LINQ to join work orders with statuses, technicians, and work types

                var query = from wo in workOrders
                            join tech in technicians on wo.TechnicianId equals tech.TechnicianId
                            join wt in workTypes on wo.WorkTypeId equals wt.Id
                            join st in statuses on wo.StatusId equals st.Id
                            where (wo.StartTime.HasValue && wo.EndTime.HasValue &&
                                   (DateTimeOffset)wo.StartTime.Value >= startTime &&
                                   (DateTimeOffset)wo.EndTime.Value <= endTime) &&
                                  (workType == "all" || wt.Name.Equals(workType, StringComparison.OrdinalIgnoreCase)) &&
                                  (status == "all" || st.Name.Equals(status, StringComparison.OrdinalIgnoreCase))
                            select new WorkOrderDetails
                            {
                                WorkOrderName = wo.WorkOrderName,
                                Technician = tech.Name,
                                WorkType = wt.Name,
                                Status = st.Name,
                                EndTime = wo.EndTime.HasValue ? (DateTimeOffset)wo.EndTime.Value : (DateTimeOffset?)null,
                                StartTime = wo.StartTime.HasValue ? (DateTimeOffset)wo.StartTime.Value : (DateTimeOffset?)null
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                throw;
            }
        }
    }
}
