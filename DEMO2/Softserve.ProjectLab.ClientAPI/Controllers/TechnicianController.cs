using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;
using System.Net;

namespace Softserve.ProjectLab.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianController : Controller
    {
        private readonly ITechnicianService _technicianService;

        public TechnicianController(ITechnicianService technicianService)
        {
            _technicianService = technicianService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Technician[] workOrders = await _technicianService.GetTechniciansAsync();
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{technicianID}")]
        public async Task<IActionResult> Get(int technicianID)
        {
            try
            {
                Technician technician = await _technicianService.GetTechnicianAsync(technicianID);
                if (technician == null)
                {
                    return NotFound();
                }

                return Ok(technician);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TechnicianByName/{technicianName}")]
        public async Task<IActionResult> Get(string technicianName)
        {
            try
            {
               List<TechnicianDetails> technicians = await _technicianService.GetTechnicianByNameAsync(technicianName);
                if (technicians == null)
                {
                    return NotFound();
                }

                return Ok(technicians);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

		[HttpGet("/Technician/List")]
		public async Task<IActionResult> List() 
        {             
            var technicians = await _technicianService.GetTechniciansAsync();
            
            return View(technicians);
        }

		[HttpGet("/Technician/Details/{technicianID}")]
		public async Task<IActionResult> Details(int technicianID)
        {
            var technician = await _technicianService.GetTechnicianAsync(technicianID);
            if (technician == null)
            {
				return NotFound();
			}

            var technicianWithWorkOrders = await _technicianService.GetTechnicianByNameAsync(technician.Name);
            
            var query = from tech in technicianWithWorkOrders
                        where technician.TechnicianId == technicianID
                        select tech;

            var res = query.First();

            return View(res);
        }
	}
 }
