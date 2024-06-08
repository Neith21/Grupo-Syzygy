using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.ClinicalExams;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicalExamController : ControllerBase
    {
        private readonly IClinicalExamRepository _clinicalExamRepository;
        private readonly IValidator<ClinicalExamModel> _validator;

        public ClinicalExamController(IClinicalExamRepository clinicalExamRepository, IValidator<ClinicalExamModel> validator)
        {
            _clinicalExamRepository = clinicalExamRepository;
            _validator = validator;
        }

        // GET: api/<ClinicalExamController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clinicalExams = await _clinicalExamRepository.GetAllClinicalExamsAsync();
            return Ok(clinicalExams);
        }

        // GET api/<ClinicalExamController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var clinicalExam = await _clinicalExamRepository.GetClinicalExamsByIdAsync(id);
            if (clinicalExam == null)
            {
                return NotFound();
            }
            return Ok(clinicalExam);
        }

        // POST api/<ClinicalExamController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClinicalExamModel clinicalExam)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(clinicalExam);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _clinicalExamRepository.AddClinicalExamsAsync(clinicalExam);
            return CreatedAtAction(nameof(Get), new { id = clinicalExam.ClinicalExamId }, clinicalExam);
        }

        // PUT api/<ClinicalExamController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClinicalExamModel clinicalExam)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(clinicalExam);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            if (id != clinicalExam.ClinicalExamId)
            {
                return BadRequest("ID mismatch");
            }

            var clinicalExamEditable = await _clinicalExamRepository.GetClinicalExamsByIdAsync(id);
            if (clinicalExamEditable == null)
            {
                return NotFound();
            }

            await _clinicalExamRepository.EditClinicalExamsAsync(clinicalExam);
            return NoContent();
        }

        // DELETE api/<ClinicalExamController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clinicalExam = await _clinicalExamRepository.GetClinicalExamsByIdAsync(id);
            if (clinicalExam == null)
            {
                return NotFound();
            }

            await _clinicalExamRepository.DeleteClinicalExamsAsync(id);
            return NoContent();
        }
    }
}
