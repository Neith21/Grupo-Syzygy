using FluentValidation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
    public class DiagnosticsValidator : AbstractValidator<DiagnosticsModel>
    {
        public DiagnosticsValidator() 
        {
            RuleFor(x => x.DiagnosticResult)
            .NotEmpty().WithMessage("Los resultados del diágnostico no pueden quedar vácios.")
            .Must(BeValidJson).WithMessage("El resultado del diágnostico debe ser un JSON válido.");

            RuleFor(x => x.DiagnosticDate)
            .NotEmpty().WithMessage("La fecha del diágnostico es obligatoria.");

            RuleFor(x => x.DiagnosticObservations)
            .NotEmpty().WithMessage("Las observaciones del diágnostico no pueden quedar vácias.");

            RuleFor(x => x.VeterinarianId)
            .NotEmpty().WithMessage("El ID del veterinario es obligatorio")
            .Must(id => id > 0).WithMessage("El ID del examen clínico debe ser mayor que 0");

            RuleFor(x => x.ClinicalExamId)
            .NotEmpty().WithMessage("El ID del examen clínico es obligatorio")
            .Must(id => id > 0).WithMessage("El ID del examen clínico debe ser mayor que 0");

        }

        private bool BeValidJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                return false;

            try
            {
                JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}
