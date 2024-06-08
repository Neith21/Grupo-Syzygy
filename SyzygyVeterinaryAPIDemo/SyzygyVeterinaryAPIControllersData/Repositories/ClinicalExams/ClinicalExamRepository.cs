using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.ClinicalExams
{
    public class ClinicalExamRepository : IClinicalExamRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public ClinicalExamRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<ClinicalExamModel>> GetAllClinicalExamsAsync()
        {
            var clinicalExams = await _dataAccess.GetDataForTwoForeignAsync<ClinicalExamModel, AnimalModel, LabTechnicianModel, dynamic>(
                "dbo.spClinicalExams_GetAll",
                new { },
                (clinicalExam, animal, labTech) =>
                {
                    clinicalExam.Animal = animal;
                    clinicalExam.LabTechnician = labTech;
                    return clinicalExam;
                },
                splitOn: "AnimalId,LabTechnicianId"
            );

            return clinicalExams;
        }

        public async Task<ClinicalExamModel?> GetClinicalExamsByIdAsync(int id)
        {
            var clinicalExams = await _dataAccess.GetDataAsync<ClinicalExamModel, dynamic>(
                "dbo.spClinicalExams_GetById",
                new { ClinicalExamId = id }
            );

            return clinicalExams.FirstOrDefault();
        }

        public async Task AddClinicalExamsAsync(ClinicalExamModel clinicalExam)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spClinicalExams_Insert",
                new { clinicalExam.ClinicalExamDate, clinicalExam.AnimalId, clinicalExam.LabTechnicianId }
            );
        }

        public async Task EditClinicalExamsAsync(ClinicalExamModel clinicalExam)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spClinicalExams_Update",
                new { clinicalExam.ClinicalExamId, clinicalExam.ClinicalExamDate, clinicalExam.AnimalId, clinicalExam.LabTechnicianId }
            );
        }

        public async Task DeleteClinicalExamsAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spClinicalExams_Delete",
                new { ClinicalExamId = id }
            );
        }
    }
}
