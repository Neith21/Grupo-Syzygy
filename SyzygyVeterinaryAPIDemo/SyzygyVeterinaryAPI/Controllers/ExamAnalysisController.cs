using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.ExamAnalyses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
	[Route("api/ExamAnalyses")]
	[ApiController]
	public class ExamAnalysisController : ControllerBase
	{
		private readonly IExamAnalysisRepository _examAnalysisRepository;
		private readonly IValidator<ExamAnalysisModel> _validator;

		public ExamAnalysisController(IExamAnalysisRepository examAnalysisRepository, IValidator<ExamAnalysisModel> validator)
		{
			_examAnalysisRepository = examAnalysisRepository;
			_validator = validator;
		}

		// GET: api/<ExamAnalysisController>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var examAnalyses = await _examAnalysisRepository.GetAllExamAnalysesAsync();
			return Ok(examAnalyses);
		}

		// GET api/<ExamAnalysisController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var examAnalysis = await _examAnalysisRepository.GetExamAnalysisByIdAsync(id);
			if (examAnalysis == null)
			{
				return NotFound();
			}
			return Ok(examAnalysis);
		}

		// POST api/<ExamAnalysisController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ExamAnalysisModel examAnalysis)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(examAnalysis);
			if (!validationResult.IsValid)
				return UnprocessableEntity(validationResult);

			await _examAnalysisRepository.AddExamAnalysisAsync(examAnalysis);
			return CreatedAtAction(nameof(Get), new { id = examAnalysis.ExamAnalysisId }, examAnalysis);
		}

		// PUT api/<ExamAnalysisController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] ExamAnalysisModel examAnalysis)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(examAnalysis);
			if (!validationResult.IsValid)
				return UnprocessableEntity(validationResult);

			if (id != examAnalysis.ExamAnalysisId)
			{
				return BadRequest("ID mismatch");
			}

			var examAnalysisEditable = await _examAnalysisRepository.GetExamAnalysisByIdAsync(id);
			if (examAnalysisEditable == null)
			{
				return NotFound();
			}

			await _examAnalysisRepository.EditExamAnalysisAsync(examAnalysis);
			return NoContent();
		}

		// DELETE api/<ExamAnalysisController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var examAnalysis = await _examAnalysisRepository.GetExamAnalysisByIdAsync(id);
			if (examAnalysis == null)
			{
				return NotFound();
			}

			await _examAnalysisRepository.DeleteExamAnalysisAsync(id);
			return NoContent();
		}
	}
}
