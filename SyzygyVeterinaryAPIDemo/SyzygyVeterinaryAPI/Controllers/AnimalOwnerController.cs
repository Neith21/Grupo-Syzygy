using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.AnimalOwners;

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/AnimalOwners")]
    [ApiController]
    public class AnimalOwnerController : ControllerBase
    {
        private readonly IAnimalOwnerRepository _animalOwnerRepository;
        private readonly IValidator<AnimalOwnerModel> _validator;

        public AnimalOwnerController(IAnimalOwnerRepository animalOwnerRepository, IValidator<AnimalOwnerModel> validator)
        {
            _animalOwnerRepository = animalOwnerRepository;
            _validator = validator;
        }

        // GET: api/<AnimalOwnerController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var animalOwners = await _animalOwnerRepository.GetAllAnimalOwnersAsync();
            return Ok(animalOwners);
        }

        // GET api/<AnimalOwnerController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var animalOwner = await _animalOwnerRepository.GetAnimalOwnerByIdAsync(id);
            if (animalOwner == null)
            {
                return NotFound();
            }
            return Ok(animalOwner);
        }

        // POST api/<AnimalOwnerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimalOwnerModel animalOwner)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(animalOwner);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _animalOwnerRepository.AddAnimalOwnerAsync(animalOwner);
            return CreatedAtAction(nameof(Get), new { id = animalOwner.AnimalOwnerId }, animalOwner);
        }

        // PUT api/<AnimalOwnerController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AnimalOwnerModel animalOwner)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(animalOwner);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            if (id != animalOwner.AnimalOwnerId)
            {
                return BadRequest("ID mismatch");
            }

            var animalOwnerEditable = await _animalOwnerRepository.GetAnimalOwnerByIdAsync(id);
            if (animalOwnerEditable == null)
            {
                return NotFound();
            }

            await _animalOwnerRepository.EditAnimalOwnerAsync(animalOwner);
            return NoContent();
        }

        // DELETE api/<AnimalOwnerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var animalOwner = await _animalOwnerRepository.GetAnimalOwnerByIdAsync(id);
            if (animalOwner == null)
            {
                return NotFound();
            }

            await _animalOwnerRepository.DeleteAnimalOwnerAsync(id);
            return NoContent();
        }
    }
}
