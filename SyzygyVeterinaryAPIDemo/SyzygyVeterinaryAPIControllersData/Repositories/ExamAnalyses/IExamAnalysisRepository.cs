using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.ExamAnalyses
{
	public interface IExamAnalysisRepository
	{
		Task AddExamAnalysisAsync(ExamAnalysisModel examAnalysis);
		Task DeleteExamAnalysisAsync(int id);
		Task EditExamAnalysisAsync(ExamAnalysisModel examAnalysis);
		Task<IEnumerable<ExamAnalysisModel>> GetAllExamAnalysesAsync();
		Task<ExamAnalysisModel?> GetExamAnalysisByIdAsync(int id);
	}
}
