using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;

namespace Softserve.ProjectLab.ClientAPI.Controllers
{
	[Route("/api/[controller]")]
	[ApiController]
	public class WorkOrderDetailsController : Controller
	{
		private readonly IWorkOrderDetailsService _workOrderDetailsService;

		public WorkOrderDetailsController(IWorkOrderDetailsService workOrderDetailsService)
		{
			_workOrderDetailsService = workOrderDetailsService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(List<WorkOrderDetails>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get(DateTimeOffset startTime, DateTimeOffset endTime, string workType, string status)
		{
			try
			{
				var workOrderDetails = await _workOrderDetailsService.GetWorkOrderDetailsAsync(startTime, endTime, workType, status);
				return Ok(workOrderDetails);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("/api/AllWorkOrders")]
		[ProducesResponseType(typeof(List<WorkOrderDetails>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get()
		{
			try
			{
				var workOrderDetails = await _workOrderDetailsService.GetWorkOrderDetailsAsync();
				return Ok(workOrderDetails);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{workOrderName}")]
		[ProducesResponseType(typeof(WorkOrderDetails), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get(string workOrderName)
		{
			try
			{
				var workOrderDetails = await _workOrderDetailsService.GetWorkOrderDetailsByNameAsync(workOrderName);
				if (workOrderDetails == null)
				{
					return NotFound();
				}

				return Ok(workOrderDetails);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("/WorkOrder/List")]
		public async Task<IActionResult> List()
		{
			var viewModel = await _workOrderDetailsService.GetWorkOrderViewModelAsync();

			var workOrders = viewModel.WorkOrders;

			ViewBag.Statuses = viewModel.Statuses;
			ViewBag.WorkTypes = viewModel.WorkTypes;

			return View(workOrders);
		}

		[HttpGet("statuses")]
		[ProducesResponseType(typeof(Status[]), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> GetStatuses()
		{
			try
			{
				var statuses = await _workOrderDetailsService.GetStatusesAsync();
				return Ok(statuses);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("workTypes")]
		[ProducesResponseType(typeof(WorkType[]), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> GetWorkTypes()
		{
			try
			{
				var workTypes = await _workOrderDetailsService.GetWorkTypesAsync();
				return Ok(workTypes);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
