using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkOrderDetailsService : IWorkOrderDetailsService
    {
        private readonly IStatusService _statusService;
        private readonly ITechnicianService _technicianService;
        private readonly IWorkTypeService _workTypeService;
        private readonly IWorkOrderService _workOrderService;
        public WorkOrderDetailsService(IStatusService statusService, ITechnicianService technicianService, IWorkTypeService workTypeService, IWorkOrderService workOrderService)
        {
            _statusService = statusService;
            _technicianService = technicianService;
            _workTypeService = workTypeService;
            _workOrderService = workOrderService;
        }
        public async Task<List<WorkOrderDetails>> GetWorkOrderDetailsAsync()
        {
            try
            {
                var workOrdersTask = _workOrderService.GetWorkOrdersAsync();
                var techniciansTask = _technicianService.GetTechniciansAsync();
                var statusesTask = _statusService.GetStatusesAsync();
                var workTypesTask = _workTypeService.GetWorkTypesAsync();

                await Task.WhenAll(workOrdersTask, statusesTask, techniciansTask, workTypesTask);

                var workOrders = workOrdersTask.Result;
                var technicians = techniciansTask.Result;
                var statuses = statusesTask.Result;
                var workTypes = workTypesTask.Result;

                var query = from wo in workOrders
                            join tech in technicians on wo.TechnicianId equals tech.TechnicianId
                            join wt in workTypes on wo.WorkTypeId equals wt.Id
                            join st in statuses on wo.StatusId equals st.Id
                            select new WorkOrderDetails
                            {
                                WorkOrderName = wo.WorkOrderName,
                                Technician = tech.Name,
                                WorkType = wt.Name,
                                EndTime = wo.EndTime.HasValue ? wo.EndTime.Value : (DateTimeOffset?)null,
                                StartTime = wo.StartTime.HasValue ? wo.StartTime.Value : (DateTimeOffset?)null
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                throw;
            }
        }

        public async Task<List<WorkOrderDetails>> GetWorkOrderDetailsAsync(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status)
        {
            try
            {
                var workOrdersTask = _workOrderService.GetWorkOrdersAsync();
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
                                   wo.StartTime.Value >= startTime &&
                                   wo.EndTime.Value <= endTime) &&
                                  (workType == "all" || wt.Name.Equals(workType, StringComparison.OrdinalIgnoreCase)) &&
                                  (status == "all" || st.Name.Equals(status, StringComparison.OrdinalIgnoreCase))
                            select new WorkOrderDetails
                            {
                                WorkOrderName = wo.WorkOrderName,
                                Technician = tech.Name,
                                WorkType = wt.Name,
                                EndTime = wo.EndTime.HasValue ? wo.EndTime.Value : (DateTimeOffset?)null,
                                StartTime = wo.StartTime.HasValue ? wo.StartTime.Value : (DateTimeOffset?)null
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                throw;
            }
        }


        public Task<List<WorkOrderDetails>> GetWorkOrderDetailsByTechnicianAsync(string technicianName)
        {
            throw new NotImplementedException();
        }
    }
}
