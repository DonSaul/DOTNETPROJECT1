﻿using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{

    public class WorkOrderService : IWorkOrderService
    {
        private readonly HttpClient _client;
        private readonly IStatusService _statusService;
        private readonly ITechnicianService _technicianService;
        private readonly IWorkTypeService _workTypeService;
        public WorkOrderService(IHttpClientFactory httpClientFactory, IStatusService statusService, ITechnicianService technicianService, IWorkTypeService workTypeService)
        {
            _client = httpClientFactory.CreateClient("apiClient");
            _statusService = statusService;
            _technicianService = technicianService;
            _workTypeService = workTypeService;
        }

        public async Task<WorkOrder[]> GetWorkOrdersAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/WorkOrder");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error al obtener las órdenes de trabajo");
            }

            string body = await response.Content.ReadAsStringAsync();
            WorkOrder[] workOrders = JsonConvert.DeserializeObject<WorkOrder[]>(body);

            return workOrders;
        }
        public async Task<WorkOrder> GetWorkOrderAsync(string workOrderName)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/WorkOrder/{workOrderName}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Error al obtener la orden de trabajo con el nombre: {workOrderName}");
            }

            string body = await response.Content.ReadAsStringAsync();
            WorkOrder workOrder = JsonConvert.DeserializeObject<WorkOrder>(body);

            return workOrder;
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
                        where (wo.StartTime.HasValue && wo.EndTime.HasValue &&
                               (DateTimeOffset)wo.StartTime.Value >= startTime &&
                               (DateTimeOffset)wo.EndTime.Value <= endTime) &&
                              (workType == "all" || wt.Name == workType) &&
                              (status == "all" || st.Name == status)
                        select new WorkOrderDetails
                        {
                            WorkOrderName = wo.WorkOrderName,
                            Technician = tech.Name,
                            WorkType = wt.Name,
                            Status = st.Name,
                            EndTime = wo.EndTime.HasValue ? (DateTimeOffset)wo.EndTime.Value : (DateTimeOffset?)null,
                            StartTime = wo.StartTime.HasValue ? (DateTimeOffset)wo.StartTime.Value : (DateTimeOffset?)null
                        };

            return query.ToArray();

        }


    }
}
