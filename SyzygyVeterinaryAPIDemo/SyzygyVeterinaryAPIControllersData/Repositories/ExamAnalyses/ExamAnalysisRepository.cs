using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.ExamAnalyses
{
	public class ExamAnalysisRepository : IExamAnalysisRepository
	{
		private readonly IDbDataAccess _dataAccess;

		public ExamAnalysisRepository(IDbDataAccess dataAccess)
		{
			_dataAccess = dataAccess;
		}

		public async Task<IEnumerable<ExamAnalysisModel>> GetAllExamAnalysesAsync()
		{
			var examAnalyses = await _dataAccess.GetDataForOneForeignAsync<ExamAnalysisModel, ClinicalExamModel, dynamic>(
				"dbo.spExamAnalyses_GetAll",
				new { },
				(examAnalysis, clinicalExam) =>
				{
					examAnalysis.ClinicalExam = clinicalExam;
					return examAnalysis;
				},
				splitOn: "ClinicalExamId"
			);

			return examAnalyses;
		}

		public async Task<ExamAnalysisModel?> GetExamAnalysisByIdAsync(int id)
		{
			var examAnalyses = await _dataAccess.GetDataAsync<ExamAnalysisModel, dynamic>(
				"dbo.spExamAnalyses_GetById",
				new { ExamAnalysisId = id }
			);

			return examAnalyses.FirstOrDefault();
		}

		public async Task AddExamAnalysisAsync(ExamAnalysisModel examAnalysis)
		{
			await _dataAccess.SaveDataAsync(
				"dbo.spExamAnalyses_Insert",
				new { examAnalysis.AnalysisType, examAnalysis.ResultData, examAnalysis.ClinicalExamId }
			);
		}

		public async Task EditExamAnalysisAsync(ExamAnalysisModel examAnalysis)
		{
			await _dataAccess.SaveDataAsync(
				"dbo.spExamAnalyses_Update",
				new { examAnalysis.ExamAnalysisId, examAnalysis.AnalysisType, examAnalysis.ResultData, examAnalysis.ClinicalExamId }
			);
		}

		public async Task DeleteExamAnalysisAsync(int id)
		{
			await _dataAccess.SaveDataAsync(
				"dbo.spExamAnalyses_Delete",
				new { ExamAnalysisId = id }
			);
		}
	}
}
