using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public interface IWorkOrderDetailsService
    {
        Task<List<WorkOrderDetails>> GetWorkOrderDetailsAsync();
        Task<List<WorkOrderDetails>> GetWorkOrderDetailsAsync(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status);
        Task<List<WorkOrderDetails>> GetWorkOrderDetailsByTechnicianAsync(string technicianName);
		Task<WorkOrderViewModel> GetWorkOrderViewModelAsync();
    }
}

