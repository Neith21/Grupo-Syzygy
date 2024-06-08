
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.ClinicalExams
{
    public interface IClinicalExamRepository
    {
        Task AddClinicalExamsAsync(ClinicalExamModel clinicalExam);
        Task DeleteClinicalExamsAsync(int id);
        Task EditClinicalExamsAsync(ClinicalExamModel clinicalExam);
        Task<IEnumerable<ClinicalExamModel>> GetAllClinicalExamsAsync();
        Task<ClinicalExamModel?> GetClinicalExamsByIdAsync(int id);
    }
}