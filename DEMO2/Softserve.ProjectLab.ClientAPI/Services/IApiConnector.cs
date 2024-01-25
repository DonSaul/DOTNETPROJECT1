using Softserve.ProjectLab.ClientAPI.Models;

namespace Softserve.ProjectLab.ClientAPI.Services
{
    public interface IApiConnector
    {
        Task<T> GetAsync<T>(string endpoint);
    }

}
