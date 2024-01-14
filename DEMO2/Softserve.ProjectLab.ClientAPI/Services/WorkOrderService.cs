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
            return await _apiConnector.GetAsync<WorkOrder[]>(ApiUrls.GetAllWorkOrders);
        }
        public async Task<WorkOrder> GetWorkOrderAsync(string workOrderName)
        {
            return await _apiConnector.GetAsync<WorkOrder>(ApiUrls.GetWorkOrderByName + workOrderName);
        }

        public async Task<WorkOrderDetails[]> GetWorkOrdersAsync(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status)
        {
            WorkOrder[] workOrders = await GetWorkOrdersAsync();
            Status[] statuses = await _statusService.GetStatusesAsync();
            Technician[] technicians = await _technicianService.GetTechniciansAsync();
            WorkType[] workTypes = await _workTypeService.GetWorkTypesAsync();

            // Usa LINQ para unir las órdenes de trabajo con los estados, los técnicos y los tipos de trabajo
            var query = from wo in workOrders
                        join tech in technicians on wo.TechnicianId equals tech.TechnicianId
                        join wt in workTypes on wo.WorkTypeId equals wt.Id
                        join st in statuses on wo.StatusId equals st.Id
                        where (wo.StartTime >= startTime && wo.EndTime <= endTime) &&
                              (wt.Name == "all" || wo.WorkTypeId == wt.Id) &&
                              (st.Name == "all" || wo.StatusId == st.Id)
                        select new WorkOrderDetails
                        {
                            WorkOrderName = wo.WorkOrderName,
                            Technician = tech.Name,
                            WorkType = wt.Name,
                            Status = st.Name,
                            EndTime = wo.EndTime,
                            StartTime = wo.StartTime
                        };

            return query.ToArray();
        }


    }
}
