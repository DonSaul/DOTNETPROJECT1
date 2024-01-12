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
        private readonly IWorkOrderService _workOrderService;

        public WorkOrderController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                WorkOrder[] workOrders = await _workOrderService.GetWorkOrdersAsync();
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{workOrderName}")]
        public async Task<IActionResult> Get(string workOrderName)
        {
            try
            {
                WorkOrder workOrder = await _workOrderService.GetWorkOrderAsync(workOrderName);
                if (workOrder == null)
                {
                    return NotFound();
                }

                return Ok(workOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> Get(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status)
        {
            try
            {
                WorkOrderDetails[] workOrders = await _workOrderService.GetWorkOrdersAsync(startTime, endTime, workType, status);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}