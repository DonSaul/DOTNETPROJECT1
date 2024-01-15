using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkTypeService : IWorkTypeService
    {
        private readonly ApiConnector _apiConnector;

        public WorkTypeService(ApiConnector apiConnector)
        {
            _apiConnector = apiConnector;
        }

        public async Task<WorkType[]> GetWorkTypesAsync()
        {
            return await _apiConnector.GetAsync<WorkType[]>(ApiUrls.GetWorkType);
        }
    }

}
