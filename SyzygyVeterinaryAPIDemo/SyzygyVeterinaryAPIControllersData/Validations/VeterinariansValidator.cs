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
    public class VeterinariansValidator : AbstractValidator<VeterinariansModel>
    {
        public VeterinariansValidator()
        {
            RuleFor(x => x.VeterinarianName)
            .NotEmpty().WithMessage("El nombre del veterinario es obligatorio")
            .Matches(@"^[^{}<>]*$").WithMessage("El nombre no puede contener {, }, < o >")
            .Matches(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El nombre solo puede contener letras y espacios.")
            .MaximumLength(255).WithMessage("El nombre no puede exceder 255 caracteres");

            RuleFor(x => x.VeterinarianSpecialization)
            .NotEmpty().WithMessage("Debe especificar la especialización del veterinario.")
            .Matches(@"^[^{}<>]*$").WithMessage("No se permiten carácteres como {, }, < o >")
            .Matches(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑ]+$").WithMessage("La especialización solo puede contener letras y espacios.")
            .MaximumLength(255).WithMessage("La especialización no puede exceder 255 caracteres");
        }
    }
}
