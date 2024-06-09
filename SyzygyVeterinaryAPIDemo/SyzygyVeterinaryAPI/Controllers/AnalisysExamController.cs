using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Services.AnalisysExams;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AnalisysExamController : ControllerBase
	{
		private readonly IAnalisysExamService _analisysExamService;

		public AnalisysExamController(IAnalisysExamService analisysExamService)
		{
			_analisysExamService = analisysExamService;
		}

		// GET api/<AnalisysExamController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> Get(int examID)
		{
			try
			{
				var result = await _analisysExamService.AnalisysExams(examID);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
	}
}
