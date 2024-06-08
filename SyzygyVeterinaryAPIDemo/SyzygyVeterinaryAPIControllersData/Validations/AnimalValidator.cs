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
    public class AnimalValidator : AbstractValidator<AnimalModel>
    {
        public AnimalValidator()
        {
            RuleFor(x => x.AnimalName)
                .NotEmpty().WithMessage("El nombre del animal es obligatorio")
                .Matches(@"^[^{}<>]*$").WithMessage("El nombre del animal no puede contener {, }, < o >")
                .Matches(@"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ.,;:()\-]+$").WithMessage("El nombre del animal solo puede contener letras, números y espacios")
                .MaximumLength(100).WithMessage("El nombre del animal no puede exceder 100 caracteres");

            RuleFor(x => x.AnimalAge)
                .NotEmpty().WithMessage("La edad del animal es obligatoria");

            RuleFor(x => x.AnimalGender)
                .NotEmpty().WithMessage("El género del animal es obligatorio")
                .Must(gender => gender == 'M' || gender == 'F').WithMessage("El género del animal debe ser 'M' o 'F'");

            RuleFor(x => x.AnimalOwnerId)
                .NotEmpty().WithMessage("El dueño del animal es obligatorio");

            RuleFor(x => x.SpeciesId)
                .NotEmpty().WithMessage("La especie es obligatorio");
        }
    }
}
