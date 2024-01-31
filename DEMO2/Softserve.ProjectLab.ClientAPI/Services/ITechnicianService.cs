using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public interface ITechnicianService
    {
        Task<Technician[]> GetTechniciansAsync();
        Task<Technician> GetTechnicianAsync(int technicianId);
        Task<List<TechnicianDetails>> GetTechnicianByNameAsync(string technicianName);
    }
}
