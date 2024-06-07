using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
	public class ReferenceValueValidator : AbstractValidator<ReferenceValueModel>
	{
		public ReferenceValueValidator()
		{
			RuleFor(x => x.AgeRange)
				.NotEmpty().WithMessage("El rango de edad es obligatorio")
				.Matches(@"^\d{1,3}-\d{1,3}$").WithMessage("El rango de edad debe tener el formato 000-000");

			RuleFor(x => x.AnalysisType)
				.NotEmpty().WithMessage("El tipo de análisis es obligatorio")
				.Matches(@"^[^{}<>]*$").WithMessage("El tipo de análisis no puede contener {, }, < o >")
				.Matches(@"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ.,;:()\-]+$").WithMessage("El tipo de análisis solo puede contener letras, números y espacios")
				.MaximumLength(255).WithMessage("El tipo de análisis no puede exceder 255 caracteres");

			RuleFor(x => x.ReferenceData)
				.NotEmpty().WithMessage("Los datos de referencia son obligatorios")
				.Must(BeValidJson).WithMessage("Los datos de referencia deben ser un JSON válido");

			RuleFor(x => x.SpeciesId)
				.NotEmpty().WithMessage("El ID de la especie es obligatorio")
				.Must(id => id > 0).WithMessage("El ID de la especie debe ser mayor que 0");
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
