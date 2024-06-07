using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.Veterinaries;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/Veterinarians")]
    [ApiController]
    public class VeterinariansController : ControllerBase
    {
        private readonly IVeterinariansRepository _veterinariansRepository;
        private readonly IValidator<VeterinariansModel> _validator;

        public VeterinariansController(IVeterinariansRepository veterinariansRepository, IValidator<VeterinariansModel> validator)
        {
            _veterinariansRepository = veterinariansRepository;
            _validator = validator;
        }

        // GET: api/<VeterinariansController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Veterinarians = await _veterinariansRepository.GetAllVeterinariansAsync();
            return Ok(Veterinarians);
        }

        // GET api/<VeterinariansController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Veterinarians = await _veterinariansRepository.GetVeterinariansByIdAsync(id);
            if (Veterinarians == null)
            {
                return NotFound();
            }
            return Ok(Veterinarians);
        }

        // POST api/<VeterinariansController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VeterinariansModel veterinarian)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(veterinarian);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _veterinariansRepository.AddVeterinariansAsync(veterinarian);
            return CreatedAtAction(nameof(Get), new { id = veterinarian.VeterinarianId}, veterinarian);
        }

        // PUT api/<VeterinariansController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] VeterinariansModel veterinarian)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(veterinarian);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            if (id != veterinarian.VeterinarianId)
            {
                return BadRequest("ID mismatch");
            }

            var VeterinarianEditable = await _veterinariansRepository.GetVeterinariansByIdAsync(id);
            if (VeterinarianEditable == null)
            {
                return NotFound();
            }

            await _veterinariansRepository.EditVeterinariansAsync(veterinarian);
            return NoContent();
        }


        // DELETE api/<VeterinariansController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var veterinarian = await _veterinariansRepository.GetVeterinariansByIdAsync(id);
            if (veterinarian == null)
            {
                return NotFound();
            }

            await _veterinariansRepository.DeleteVeterinariansAsync(id);
            return NoContent();
        }
    }
}
