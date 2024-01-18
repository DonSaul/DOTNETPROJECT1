using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public interface IWorkOrderService
    {
        Task<WorkOrder[]> GetWorkOrdersAsync();
        Task<WorkOrder> GetWorkOrderAsync(string WorkOrderName);
        Task<List<WorkOrderDetails>> GetWorkOrdersAsync(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status);
        Task<List<ReportData>> GetWorkOrderReports();
    }
}
