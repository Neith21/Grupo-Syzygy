using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SyzygyVeterinaryAPIControllersData.Repositories.Diagnostic;

namespace SyzygyVeterinaryAPIControllersData.Services.AnalisysExams
{
	public class AnalisysExamService : IAnalisysExamService
	{
		private readonly IDbDataAccess _dataAccess;
		private readonly IDiagnosticsRepository _diagnosticsRepository;

		public AnalisysExamService(IDbDataAccess dataAccess, IDiagnosticsRepository diagnosticsRepository)
		{
			_dataAccess = dataAccess;
			_diagnosticsRepository = diagnosticsRepository;
		}

		public async Task<List<string>> AnalisysExams(int examID)
		{
			int lenghtExamRef = 0, clinicalExamID = 0;
			float diff;
			string specieName = string.Empty;
			string[] typesExams = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty];
			List<string> examJsonList = new List<string>();
			List<string> examRefJsonList = new List<string>();
			List<string> diagnosticJsonList = new List<string>();

			var dataExamTask = GetExamAnalysisByIdAsync(examID);
			await Task.WhenAll(dataExamTask);
			var dataExam = dataExamTask.Result;

			if (dataExam.Count != 0)
			{
				var allDataClinicalExam = GetClinicalExam(dataExam.FirstOrDefault().ClinicalExamId);
				await Task.WhenAll(allDataClinicalExam);
				var clinicalExam = allDataClinicalExam.Result;
				clinicalExamID = (int)clinicalExam.ClinicalExamId;

				var allDataAnimal = GetAnimals(clinicalExam.AnimalId);
				await Task.WhenAll(allDataAnimal);
				var animals = allDataAnimal.Result;

				var allDataspecie = GetSpecies(animals.SpeciesId);
				await Task.WhenAll(allDataspecie);
				var specie = allDataspecie.Result;

				specieName = specie.SpeciesName;
			}

			int i = 0;
			foreach (var exams in dataExam)
			{
				if (i < 5)
				{
					typesExams[i] = exams?.AnalysisType.ToString();
					i++;
				}
				examJsonList.Add(exams?.ResultData.ToString());
			}

			var referencesExamsTask = GetReferenceValueByIdAsync(specieName, typesExams);
			await Task.WhenAll(referencesExamsTask);
			var referencesExams = referencesExamsTask.Result;

			string typesExamConcatenated = string.Join(", ", typesExams.Where(e => !string.IsNullOrEmpty(e)));

			foreach (var examRef in referencesExams)
			{
				examRefJsonList.Add(examRef.ReferenceData.ToString());
			}

			if (examRefJsonList.Count != 0 && examJsonList.Count != 0)
			{
				for (int j = 0; j < examJsonList.Count; j++)
				{
					if (j < examRefJsonList.Count)
					{
						lenghtExamRef = j;
					}

					JObject references = JObject.Parse(examRefJsonList[lenghtExamRef]);
					JArray referenceValues = (JArray)references["references"];

					JObject results = JObject.Parse(examJsonList[j]);
					JArray resultValues = (JArray)results["results"];

					// JSON de comparación
					JObject diagnosticJson = new JObject();
					JArray diagnosticArray = new JArray();

					foreach (JObject result in resultValues)
					{
						JObject comparison = new JObject();

						foreach (var parameter in result)
						{
							string paramName = parameter.Key;
							float paramValue = (float)parameter.Value["value"];

							var reference = referenceValues[0][paramName];
							float minValue = (float)reference["minValue"];
							float maxValue = (float)reference["maxValue"];



							if (paramValue < minValue || paramValue > maxValue)
							{
								if (paramValue < minValue)
								{
									diff = paramValue - minValue;
								}
								else
								{
									diff = paramValue - maxValue;
								}

								comparison[paramName] = diff;
							}
							else
							{
								comparison[paramName] = 0;
							}

							JObject diagnosticItem = new JObject();

							diagnosticItem[paramName] = new JObject
						{
							{ "value", paramValue },
							{ "minValue", minValue },
							{ "maxValue", maxValue },
							{ "abnormalValue", comparison[paramName] },
							{ "unit", "mg/dL" }
						};

							diagnosticArray.Add(diagnosticItem);

						}

						diagnosticJson["diagnostic"] = diagnosticArray;
						diagnosticJsonList.Add(diagnosticJson.ToString().Replace("\r\n", "").Replace(" ", ""));
					}
				}

				DiagnosticsModel diagnosticsModel = new DiagnosticsModel();

				string diagnosticResultJson = string.Join(",", diagnosticJsonList);

				diagnosticsModel.DiagnosticResult = diagnosticResultJson;
				diagnosticsModel.DiagnosticDate = DateTime.Now;
				diagnosticsModel.DiagnosticObservations = "Pendiente";
				diagnosticsModel.VeterinarianId = 1;
				diagnosticsModel.ClinicalExamId = clinicalExamID;

				_diagnosticsRepository.AddDiagnosticsAsync(diagnosticsModel);

				return diagnosticJsonList;
			}
			diagnosticJsonList.Add("Surgio un Error Inesperado");
			return diagnosticJsonList;
		}

		private async Task<List<ExamAnalysisModel?>> GetExamAnalysisByIdAsync(int id)
		{
			var examAnalyses = await _dataAccess.GetDataAsync<ExamAnalysisModel, dynamic>(
				"dbo.spAnalizeExam_GetDataJsonByIdExam",
				new { ClinicalExamId = id }
			);

			return examAnalyses.ToList();
		}

		private async Task<List<ReferenceValueModel?>> GetReferenceValueByIdAsync(string speciesName, string[] typesExams)
		{
			try
			{
				dynamic parameters = new ExpandoObject();

				((IDictionary<string, object>)parameters)["@SpeciesName"] = speciesName;
				((IDictionary<string, object>)parameters)["@ExamType1"] = typesExams[0];
				((IDictionary<string, object>)parameters)["@ExamType2"] = typesExams[1];
				((IDictionary<string, object>)parameters)["@ExamType3"] = typesExams[2];
				((IDictionary<string, object>)parameters)["@ExamType4"] = typesExams[3];
				((IDictionary<string, object>)parameters)["@ExamType5"] = typesExams[4];

				var referenceValues = await _dataAccess.GetDataAsync<ReferenceValueModel, dynamic>(
					"dbo.spAnalizeExam_GetDataJsonByIdReferences",
					parameters
				);

				return referenceValues;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		private async Task<ClinicalExamModel?> GetClinicalExam(int id)
		{
			var ClinicalExams = await _dataAccess.GetDataAsync<ClinicalExamModel, dynamic>(
				"dbo.spAnalizeExam_GetClinicalExam",
				new { ClinicalExamId = id }
			);
			return ClinicalExams.FirstOrDefault();
		}

		private async Task<AnimalModel?> GetAnimals(int id)
		{
			var examAnalyses = await _dataAccess.GetDataAsync<AnimalModel, dynamic>(
				"dbo.spAnalizeExam_GetAnimals",
				new { AnimalId = id }
			);
			return examAnalyses.FirstOrDefault();
		}

		private async Task<SpeciesModel?> GetSpecies(int id)
		{
			var examAnalyses = await _dataAccess.GetDataAsync<SpeciesModel, dynamic>(
				"dbo.spAnalizeExam_GetSpecies",
				new { SpeciesId = id }
			);

			return examAnalyses.FirstOrDefault();
		}
	}
}
