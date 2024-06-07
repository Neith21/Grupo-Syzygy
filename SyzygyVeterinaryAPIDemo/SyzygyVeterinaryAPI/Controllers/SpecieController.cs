using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.Species;

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/Species")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IValidator<SpeciesModel> _validator;

        public SpeciesController(ISpeciesRepository speciesRepository, IValidator<SpeciesModel> validator)
        {
            _speciesRepository = speciesRepository;
            _validator = validator;
        }

        // GET: api/<SpeciesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var species = await _speciesRepository.GetAllSpeciesAsync();
            return Ok(species);
        }

        // GET api/<SpeciesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var species = await _speciesRepository.GetSpeciesByIdAsync(id);
            if (species == null)
            {
                return NotFound();
            }
            return Ok(species);
        }

        // POST api/<SpeciesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SpeciesModel species)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(species);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _speciesRepository.AddSpeciesAsync(species);
            return CreatedAtAction(nameof(Get), new { id = species.SpeciesId }, species);
        }

        // PUT api/<SpeciesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SpeciesModel species)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(species);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            if (id != species.SpeciesId)
            {
                return BadRequest("ID mismatch");
            }

            var existingSpecies = await _speciesRepository.GetSpeciesByIdAsync(id);
            if (existingSpecies == null)
            {
                return NotFound();
            }

            await _speciesRepository.EditSpeciesAsync(species);
            return NoContent();
        }

        // DELETE api/<SpeciesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var species = await _speciesRepository.GetSpeciesByIdAsync(id);
            if (species == null)
            {
                return NotFound();
            }

            await _speciesRepository.DeleteSpeciesAsync(id);
            return NoContent();
        }
    }
}
