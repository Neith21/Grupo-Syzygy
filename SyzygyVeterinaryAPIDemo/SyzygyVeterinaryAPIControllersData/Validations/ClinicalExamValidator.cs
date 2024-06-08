using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
    public class ClinicalExamValidator : AbstractValidator<ClinicalExamModel>
    {
        public ClinicalExamValidator()
        {
            RuleFor(x => x.ClinicalExamDate)
                .NotEmpty().WithMessage("La fecha del examen clínico es obligatoria");

            RuleFor(x => x.AnimalId)
                .NotEmpty().WithMessage("El animal es obligatorio");

            RuleFor(x => x.LabTechnicianId)
                .NotEmpty().WithMessage("El técnico de laboratorio es obligatorio");
        }
    }

}
