using FluentValidation;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
    public class SpeciesValidator : AbstractValidator<SpeciesModel>
    {
        public SpeciesValidator()
        {
            RuleFor(x => x.SpeciesName)
                .NotEmpty().WithMessage("El nombre de la especie es obligatorio")
                .Matches(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El nombre de la especie solo puede contener letras y espacios")
                .MaximumLength(100).WithMessage("El nombre de la especie no puede exceder 100 caracteres");
        }
    }
}
