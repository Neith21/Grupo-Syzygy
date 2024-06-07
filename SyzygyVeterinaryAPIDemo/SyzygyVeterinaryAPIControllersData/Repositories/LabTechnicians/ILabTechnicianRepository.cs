using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.LabTechnicians
{
    public interface ILabTechnicianRepository
    {
        Task AddLabTechnicianAsync(LabTechnicianModel labTechnician);
        Task DeleteLabTechnicianAsync(int id);
        Task EditLabTechnicianAsync(LabTechnicianModel labTechnician);
        Task<IEnumerable<LabTechnicianModel>> GetAllLabTechniciansAsync();
        Task<LabTechnicianModel?> GetLabTechnicianByIdAsync(int id);
    }
}