using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Validations
{
	public class ExamAnalysisValidator : AbstractValidator<ExamAnalysisModel>
	{
		public ExamAnalysisValidator()
		{
			RuleFor(x => x.AnalysisType)
				.NotEmpty().WithMessage("El tipo de análisis es obligatorio")
				.Matches(@"^[^{}<>]*$").WithMessage("El tipo de análisis no puede contener {, }, < o >")
				.Matches(@"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ.,;:()\-]+$").WithMessage("El tipo de análisis solo puede contener letras, números y espacios")
				.MaximumLength(255).WithMessage("El tipo de análisis no puede exceder 255 caracteres");

			RuleFor(x => x.ResultData)
				.NotEmpty().WithMessage("Los datos del resultado son obligatorios")
				.Must(BeValidJson).WithMessage("Los datos del resultado deben ser un JSON válido");

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
