using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.LabTechnicians;

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/LabTechnicians")]
    [ApiController]
    public class LabTechnicianController : ControllerBase
    {
        private readonly ILabTechnicianRepository _labTechnicianRepository;
        private readonly IValidator<LabTechnicianModel> _validator;

        public LabTechnicianController(ILabTechnicianRepository labTechnicianRepository, IValidator<LabTechnicianModel> validator)
        {
            _labTechnicianRepository = labTechnicianRepository;
            _validator = validator;
        }

        // GET: api/<LabTechnicianController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var labTechnicians = await _labTechnicianRepository.GetAllLabTechniciansAsync();
            return Ok(labTechnicians);
        }

        // GET api/<LabTechnicianController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var labTechnician = await _labTechnicianRepository.GetLabTechnicianByIdAsync(id);
            if (labTechnician == null)
            {
                return NotFound();
            }
            return Ok(labTechnician);
        }

        // POST api/<LabTechnicianController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LabTechnicianModel labTechnician)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(labTechnician);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _labTechnicianRepository.AddLabTechnicianAsync(labTechnician);
            return CreatedAtAction(nameof(Get), new { id = labTechnician.LabTechnicianId }, labTechnician);
        }

        // PUT api/<LabTechnicianController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LabTechnicianModel labTechnician)
        {
            var validationResult = await _validator.ValidateAsync(labTechnician);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult);
            }

            if (id != labTechnician.LabTechnicianId)
            {
                return BadRequest("ID mismatch");
            }

            var existingLabTechnician = await _labTechnicianRepository.GetLabTechnicianByIdAsync(id);
            if (existingLabTechnician == null)
            {
                return NotFound();
            }

            await _labTechnicianRepository.EditLabTechnicianAsync(labTechnician);
            return NoContent();
        }

        // DELETE api/<LabTechnicianController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingLabTechnician = await _labTechnicianRepository.GetLabTechnicianByIdAsync(id);
            if (existingLabTechnician == null)
            {
                return NotFound();
            }

            await _labTechnicianRepository.DeleteLabTechnicianAsync(id);
            return NoContent();
        }

    }
}


