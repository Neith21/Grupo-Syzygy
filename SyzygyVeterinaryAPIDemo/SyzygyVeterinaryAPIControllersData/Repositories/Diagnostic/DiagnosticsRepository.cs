using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Diagnostic
{
    public class DiagnosticsRepository : IDiagnosticsRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public DiagnosticsRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<DiagnosticsModel>> GetAllDiagnosticsAsync()
        {
            var diagnostics = await _dataAccess.GetDataForTwoForeignAsync<DiagnosticsModel, VeterinariansModel, ClinicalExamModel, dynamic>(
                "dbo.spDiagnostics_GetAll",
                new { },
                (diagnostic, veterinarians, exams) =>
                {
                    diagnostic.Veterinarian = veterinarians;
                    diagnostic.ClinicalExam = exams;
                    return diagnostic;
                },
                splitOn: "VeterinarianName, ClinicalExamId"
            );

            return diagnostics;
        }

        public async Task<DiagnosticsModel?> GetDiagnosticsByIdAsync(int id)
        {
            var Diagnostic = await _dataAccess.GetDataAsync<DiagnosticsModel, dynamic>(
                 "dbo.spDiagnostics_GetById",
                 new { DiagnosticId = id }
             );

            return Diagnostic.FirstOrDefault();
        }

        public async Task AddDiagnosticsAsync(DiagnosticsModel diagnostic)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spDiagnostics_Insert",
                new { diagnostic.DiagnosticResult, diagnostic.DiagnosticDate, diagnostic.DiagnosticObservations, diagnostic.VeterinarianId, diagnostic.ClinicalExamId }
            );
        }

        public async Task DeleteDiagnosticsAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spDiagnostics_Delete",
                new { DiagnosticId = id }
            );
        }

        public async Task EditDiagnosticsAsync(DiagnosticsModel diagnostic)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spDiagnostics_Update",
                new { diagnostic.DiagnosticId, diagnostic.DiagnosticResult, diagnostic.DiagnosticDate, diagnostic.DiagnosticObservations, diagnostic.VeterinarianId, diagnostic.ClinicalExamId}
            );
        }
    }
}
