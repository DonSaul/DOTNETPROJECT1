using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ApiConnector _apiConnector;

        public WorkOrderService(ApiConnector apiConnector)
        {
            _apiConnector = apiConnector;
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

        
    }
}
