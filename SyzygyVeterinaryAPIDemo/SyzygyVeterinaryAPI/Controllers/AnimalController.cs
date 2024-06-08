using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.Animals;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/Animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animalsRepository;
        private readonly IValidator<AnimalModel> _validator;

        public AnimalController(IAnimalRepository animalsRepository, IValidator<AnimalModel> validator)
        {
            _animalsRepository = animalsRepository;
            _validator = validator;
        }

        // GET: api/<AnimalController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var animals = await _animalsRepository.GetAllAnimalsAsync();
            return Ok(animals);
        }

        // GET api/<AnimalController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var animal = await _animalsRepository.GetAnimalsByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }

        // POST api/<AnimalController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimalModel animal)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(animal);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _animalsRepository.AddAnimalsAsync(animal);
            return CreatedAtAction(nameof(Get), new { id = animal.AnimalId }, animal);
        }

        // PUT api/<AnimalController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AnimalModel animal)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(animal);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            if (id != animal.AnimalId)
            {
                return BadRequest("ID mismatch");
            }

            var animalEditable = await _animalsRepository.GetAnimalsByIdAsync(id);
            if (animalEditable == null)
            {
                return NotFound();
            }

            await _animalsRepository.EditAnimalsAsync(animal);
            return NoContent();
        }

        // DELETE api/<AnimalController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _animalsRepository.GetAnimalsByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            await _animalsRepository.DeleteAnimalsAsync(id);
            return NoContent();
        }
    }
}
