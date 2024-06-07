using FluentValidation;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
    public class LabTechnicianValidator : AbstractValidator<LabTechnicianModel>
    {
        public LabTechnicianValidator()
        {
            RuleFor(x => x.LabTechnicianName)
                .NotEmpty().WithMessage("El nombre del técnico de laboratorio es obligatorio")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El nombre del técnico de laboratorio solo puede contener letras y espacios")
                .MaximumLength(100).WithMessage("El nombre del técnico de laboratorio no puede exceder 100 caracteres");

            RuleFor(x => x.LabTechnicianSpecialization)
                .NotEmpty().WithMessage("La especialización del técnico de laboratorio es obligatoria")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("La especialización del técnico de laboratorio solo puede contener letras y espacios")
                .MaximumLength(100).WithMessage("La especialización del técnico de laboratorio no puede exceder 100 caracteres");
        }
    }
}
