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
								Status = st.Name,
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
			if(endTime < startTime || startTime > endTime)
			{
                throw new ArgumentException();
            }

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
					where (wo.StartTime.HasValue && wo.EndTime.HasValue &&
						wo.StartTime.Value >= startTime &&
						wo.EndTime.Value <= endTime) &&
						(workType == "all" || wt.Name.Equals(workType, StringComparison.OrdinalIgnoreCase)) &&
						(status == "all" || st.Name.Equals(status, StringComparison.OrdinalIgnoreCase))
						select new WorkOrderDetails
						{
							WorkOrderName = wo.WorkOrderName,
							Technician = tech.Name,
							Status = st.Name,
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

		public async Task<WorkOrderDetails> GetWorkOrderDetailsByNameAsync(string WorkOrderName)
		{
			try
			{
				var workOrder = await _workOrderService.GetWorkOrderAsync(WorkOrderName);
				var technicians = await _technicianService.GetTechnicianAsync(workOrder.TechnicianId);
				var statuses = await _statusService.GetStatusesAsync();
				var workTypes = await _workTypeService.GetWorkTypesAsync();

				var status = statuses.FirstOrDefault(s => s.Id == workOrder.StatusId);
				var workType = workTypes.FirstOrDefault(wt => wt.Id == workOrder.WorkTypeId);

				return new WorkOrderDetails
				{
					WorkOrderName = workOrder.WorkOrderName,
					Technician = technicians.Name,
					WorkType = workType?.Name ?? "",
					Status = status?.Name ?? "",
					EndTime = workOrder.EndTime ?? (DateTimeOffset?)null,
					StartTime = workOrder.StartTime ?? (DateTimeOffset?)null
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error : {ex.Message}");
				throw;
			}
		}

		public async Task<WorkOrderViewModel> GetWorkOrderViewModelAsync()
		{
			var workOrdersTask = GetWorkOrderDetailsAsync();
			var statusesTask = _statusService.GetStatusesAsync();
			var workTypesTask = _workTypeService.GetWorkTypesAsync();

			await Task.WhenAll(workOrdersTask, statusesTask, workTypesTask);

			var workOrders = workOrdersTask.Result;
			var statuses = statusesTask.Result;
			var workTypes = workTypesTask.Result;

			return new WorkOrderViewModel
			{
				WorkOrders = workOrders,
				Statuses = statuses,
				WorkTypes = workTypes
			};
		}

		public Task<List<WorkOrderDetails>> GetWorkOrderDetailsByTechnicianAsync(string technicianName)
		{
			throw new NotImplementedException();
		}

		public async Task<Status[]> GetStatusesAsync()
		{
			var statuses = await _statusService.GetStatusesAsync();
			return statuses;
		}

		public async Task<WorkType[]> GetWorkTypesAsync()
		{
			var workTypes = await _workTypeService.GetWorkTypesAsync();
			return workTypes;
		}
	}
}
