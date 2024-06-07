using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.ReferenceValues;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
	[Route("api/ReferenceValues")]
	[ApiController]
	public class ReferenceValueController : ControllerBase
	{
		private readonly IReferenceValueRepository _referenceValueRepository;
		private readonly IValidator<ReferenceValueModel> _validator;

		public ReferenceValueController(IReferenceValueRepository referenceValueRepository, IValidator<ReferenceValueModel> validator)
		{
			_referenceValueRepository = referenceValueRepository;
			_validator = validator;
		}

		// GET: api/<ReferenceValueController>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var referenceValues = await _referenceValueRepository.GetAllReferenceValuesAsync();
			return Ok(referenceValues);
		}

		// GET api/<ReferenceValueController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var referenceValue = await _referenceValueRepository.GetReferenceValueByIdAsync(id);
			if (referenceValue == null)
			{
				return NotFound();
			}
			return Ok(referenceValue);
		}

		// POST api/<ReferenceValueController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ReferenceValueModel referenceValue)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(referenceValue);
			if (!validationResult.IsValid)
				return UnprocessableEntity(validationResult);

			await _referenceValueRepository.AddReferenceValueAsync(referenceValue);
			return CreatedAtAction(nameof(Get), new { id = referenceValue.ReferenceValueId }, referenceValue);
		}

		// PUT api/<ReferenceValueController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] ReferenceValueModel referenceValue)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(referenceValue);
			if (!validationResult.IsValid)
				return UnprocessableEntity(validationResult);

			if (id != referenceValue.ReferenceValueId)
			{
				return BadRequest("ID mismatch");
			}

			var referenceValueEditable = await _referenceValueRepository.GetReferenceValueByIdAsync(id);
			if (referenceValueEditable == null)
			{
				return NotFound();
			}

			await _referenceValueRepository.EditReferenceValueAsync(referenceValue);
			return NoContent();
		}

		// DELETE api/<ReferenceValueController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var referenceValue = await _referenceValueRepository.GetReferenceValueByIdAsync(id);
			if (referenceValue == null)
			{
				return NotFound();
			}

			await _referenceValueRepository.DeleteReferenceValueAsync(id);
			return NoContent();
		}
	}
}
