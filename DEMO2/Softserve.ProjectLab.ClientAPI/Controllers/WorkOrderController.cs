using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController : Controller
    {
        private readonly HttpConnector _httpConnector;

        public WorkOrderController()
        {
            _httpConnector = new HttpConnector();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            string url = ApiUrls.GetAllWorkOrders;

            try
            {
                string data = await _httpConnector.Get(url);
                return data;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        
        [HttpGet("{workOrderName}")]
        public async Task<string> Get(string workOrderName)
        {
            string url = ApiUrls.GetWorkOrderByName;

            try
            {
                string data = await _httpConnector.Get(url + workOrderName);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}