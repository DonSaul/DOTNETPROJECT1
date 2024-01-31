using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public interface IWorkTypeService
    {
        Task<WorkType[]> GetWorkTypesAsync();
    }
}
