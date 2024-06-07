using FluentValidation;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
    public class AnimalOwnerValidator : AbstractValidator<AnimalOwnerModel>
    {
        public AnimalOwnerValidator()
        {
            RuleFor(x => x.AnimalOwnerName)
                .NotEmpty().WithMessage("El nombre del propietario del animal es obligatorio")
                .Matches(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El nombre del propietario solo puede contener letras y espacios")
                .MaximumLength(100).WithMessage("El nombre del propietario no puede exceder 100 caracteres");

            RuleFor(x => x.AnimalOwnerContactInfo)
                .NotEmpty().WithMessage("La información de contacto es obligatoria")
                .Matches(@"^[^{}<>]*$").WithMessage("La información de contacto no puede contener {, }, < o >")
                .MaximumLength(255).WithMessage("La información de contacto no puede exceder 255 caracteres");
        }
    }
}
