using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Config;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApiConnector _apiConnector;
        //private readonly ApiSettings _apiSettings;

        public StatusService(ApiConnector apiConnector)
        {
            _apiConnector = apiConnector ?? throw new ArgumentNullException(nameof(apiConnector));
            //_apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        public async Task<Status[]> GetStatusesAsync()
        {
            return await _apiConnector.GetAsync<Status[]>(ApiUrls.GetStatus);
        }
    }
}
