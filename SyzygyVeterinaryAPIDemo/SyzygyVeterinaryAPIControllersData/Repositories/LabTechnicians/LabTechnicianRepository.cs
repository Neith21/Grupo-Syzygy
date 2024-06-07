using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.LabTechnicians
{
    public class LabTechnicianRepository : ILabTechnicianRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public LabTechnicianRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<LabTechnicianModel>> GetAllLabTechniciansAsync()
        {
            var labTechnicians = await _dataAccess.GetDataAsync<LabTechnicianModel, dynamic>(
                "dbo.sp_LabTechnicians_GetAll",
                new { }
            );

            return labTechnicians;
        }

        public async Task<LabTechnicianModel?> GetLabTechnicianByIdAsync(int id)
        {
            var labTechnician = await _dataAccess.GetDataAsync<LabTechnicianModel, dynamic>(
                "dbo.sp_LabTechnicians_GetById",
                new { LabTechnicianId = id }
            );

            return labTechnician.FirstOrDefault();
        }

        public async Task AddLabTechnicianAsync(LabTechnicianModel labTechnician)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_LabTechnicians_Insert",
                new { labTechnician.LabTechnicianName, labTechnician.LabTechnicianSpecialization }
            );
        }

        public async Task EditLabTechnicianAsync(LabTechnicianModel labTechnician)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_LabTechnicians_Update",
                new { labTechnician.LabTechnicianId, labTechnician.LabTechnicianName, labTechnician.LabTechnicianSpecialization }
            );
        }

        public async Task DeleteLabTechnicianAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_LabTechnicians_Delete",
                new { LabTechnicianId = id }
            );
        }
    }
}
