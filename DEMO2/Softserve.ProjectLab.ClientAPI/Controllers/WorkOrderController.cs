using Microsoft.AspNetCore.Mvc;
using Softserve.ProjectLab.ClientAPI.Services;
using System.Text;

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

        [HttpGet("all")]
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

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkOrderDetails>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> Get(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status)
        {
            try
            {
                var workOrders = await _workOrderService.GetWorkOrdersAsync(startTime, endTime, workType, status);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportWorkOrderReportsToCsv()
        {
            try
            {
                var reports = await _workOrderService.GetWorkOrderReports();
                var csvWriter = new StringBuilder();
                csvWriter.AppendLine("WorkOrderName,TechnicianName,WorkType,Status,EndTime,StartTime,CreatedDate,Duration");
                foreach (var reportData in reports)
                {
                    csvWriter.AppendLine(reportData.WorkOrderName + "," +
                                         reportData.TechnicianName + "," +
                                         reportData.WorkType + "," +
                                         reportData.Status + "," +
                                         reportData.EndTime + "," +
                                         reportData.StartTime + "," +
                                         reportData.CreatedDate + "," +
                                         reportData.Duration);
                }

                return File(Encoding.UTF8.GetBytes(csvWriter.ToString()), "text/csv", "work_orders.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}