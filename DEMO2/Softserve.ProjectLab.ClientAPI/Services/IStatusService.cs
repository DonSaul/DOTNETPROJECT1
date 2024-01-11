using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public interface IStatusService
    {
        Task<Status[]> GetStatusesAsync();
    }

}
