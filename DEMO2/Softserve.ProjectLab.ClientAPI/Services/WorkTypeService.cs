using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class WorkTypeService : IWorkTypeService
    {
        private readonly IApiConnector _apiConnector;

        public WorkTypeService(IApiConnector apiConnector)
        {
            _apiConnector = apiConnector ?? throw new ArgumentNullException(nameof(apiConnector));
        }

        public async Task<WorkType[]> GetWorkTypesAsync()
        {
            return await _apiConnector.GetAsync<WorkType[]>(ApiUrls.GetWorkType);
        }
    }

}
