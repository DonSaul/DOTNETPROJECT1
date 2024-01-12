using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Controllers;
using Softserve.ProjectLab.ClientAPI.Models;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly ApiConnector _apiConnector;

        public TechnicianService(ApiConnector apiConnector)
        {
            _apiConnector = apiConnector;
        }

        public async Task<Technician[]> GetTechniciansAsync()
        {
            return await _apiConnector.GetAsync<Technician[]>(ApiUrls.GetAllTechnicians);
        }
        public async Task<Technician> GetTechnicianAsync(int technicianId)
        {
            return await _apiConnector.GetAsync<Technician>(ApiUrls.GetTechnicianById);
        }

    }

}
