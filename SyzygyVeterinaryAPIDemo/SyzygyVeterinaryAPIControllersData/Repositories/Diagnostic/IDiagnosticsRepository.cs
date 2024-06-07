using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Diagnostic
{
    public interface IDiagnosticsRepository
    {
        Task AddDiagnosticsAsync(DiagnosticsModel diagnostic);
        Task DeleteDiagnosticsAsync(int id);
        Task EditDiagnosticsAsync(DiagnosticsModel diagnostic);
        Task<IEnumerable<DiagnosticsModel>> GetAllDiagnosticsAsync();
        Task<DiagnosticsModel?> GetDiagnosticsByIdAsync(int id);
    }
}
