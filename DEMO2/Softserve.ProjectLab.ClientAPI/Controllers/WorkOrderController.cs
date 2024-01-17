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
        [ProducesResponseType(typeof(List<WorkOrder>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var workOrders = await _workOrderService.GetWorkOrdersAsync();
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{workOrderName}")]
        [ProducesResponseType(typeof(WorkOrder), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string workOrderName)
        {
            try
            {
                var workOrder = await _workOrderService.GetWorkOrderAsync(workOrderName);
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

    }
}