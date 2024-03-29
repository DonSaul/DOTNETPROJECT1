﻿using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IApiConnector _apiConnector;
        private readonly IStatusService _statusService;
        private readonly ITechnicianService _technicianService;
        private readonly IWorkTypeService _workTypeService;

        public WorkOrderService(IApiConnector apiConnector, IHttpClientFactory httpClientFactory, IStatusService statusService, ITechnicianService technicianService, IWorkTypeService workTypeService)
        {
            _apiConnector = apiConnector ?? throw new ArgumentNullException(nameof(apiConnector));
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

        public async Task<List<ReportData>> GetWorkOrderReports()
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

            var query = from wo in workOrders
                join tech in technicians on wo.TechnicianId equals tech.TechnicianId
                join wt in workTypes on wo.WorkTypeId equals wt.Id
                join st in statuses on wo.StatusId equals st.Id
                where wo.EndTime.HasValue && wo.StartTime.HasValue
                select new ReportData
                {
                    WorkOrderName = wo.WorkOrderName,
                    TechnicianName = tech.Name,
                    WorkType = wt.Name,
                    Status = st.Name,
                    EndTime = wo.EndTime.Value,
                    StartTime =  wo.StartTime.Value,
                    CreatedDate = wo.CreatedDate,
                    Duration = wo.EndTime.Value - wo.StartTime.Value,
                };
            return query.ToList();
        }

    }
}
