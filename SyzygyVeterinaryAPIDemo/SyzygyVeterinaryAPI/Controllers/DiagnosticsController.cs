using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.Diagnostic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyzygyVeterinaryAPI.Controllers
{
    [Route("api/Diagnostics")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IDiagnosticsRepository _diagnosticsRepository;
        private readonly IValidator<DiagnosticsModel> _validator;

        public DiagnosticsController(IDiagnosticsRepository diagnosticsRepository, IValidator<DiagnosticsModel> validator)
        {
            _diagnosticsRepository = diagnosticsRepository;
            _validator = validator;
        }


        // GET: api/<DiagnosticsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Diagnostics = await _diagnosticsRepository.GetAllDiagnosticsAsync();
            return Ok(Diagnostics);
        }

        // GET api/<DiagnosticsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Diagnostics = await _diagnosticsRepository.GetDiagnosticsByIdAsync(id);
            if (Diagnostics == null)
            {
                return NotFound();
            }
            return Ok(Diagnostics);
        }

        // POST api/<DiagnosticsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DiagnosticsModel diagnostic)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(diagnostic);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            await _diagnosticsRepository.AddDiagnosticsAsync(diagnostic);
            return CreatedAtAction(nameof(Get), new { id = diagnostic.DiagnosticId }, diagnostic);
        }

        // PUT api/<DiagnosticsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DiagnosticsModel diagnostics)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(diagnostics);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            if (id != diagnostics.DiagnosticId)
            {
                return BadRequest("ID mismatch");
            }

            var diagnosticEditable = await _diagnosticsRepository.GetDiagnosticsByIdAsync(id);
            if (diagnosticEditable == null)
            {
                return NotFound();
            }

            await _diagnosticsRepository.EditDiagnosticsAsync(diagnostics);
            return NoContent();
        }

        // DELETE api/<DiagnosticsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var diagnostic = await _diagnosticsRepository.GetDiagnosticsByIdAsync(id);
            if (diagnostic == null)
            {
                return NotFound();
            }

            await _diagnosticsRepository.DeleteDiagnosticsAsync(id);
            return NoContent();
        }
    }
}
