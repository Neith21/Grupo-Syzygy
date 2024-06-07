using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Veterinaries
{
    public class VeterinariansRepository : IVeterinariansRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public VeterinariansRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task AddVeterinariansAsync(VeterinariansModel veterinarians)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spVeterinarians_Insert",
                new { veterinarians.VeterinarianName, veterinarians.VeterinarianSpecialization }
            );
        }

        public async Task DeleteVeterinariansAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                 "dbo.spVeterinarians_Delete",
                 new { VeterinarianId = id }
             );
        }

        public async Task EditVeterinariansAsync(VeterinariansModel veterinarians)
        {
            await _dataAccess.SaveDataAsync(
                 "dbo.spVeterinarians_Update",
                 new { veterinarians.VeterinarianId, veterinarians.VeterinarianName, veterinarians.VeterinarianSpecialization }
             );
        }

        public async Task<IEnumerable<VeterinariansModel>> GetAllVeterinariansAsync()
        {
            return await _dataAccess.GetDataAsync<VeterinariansModel, dynamic>(
                    "dbo.spVeterinarians_GetAll",
                    new { }
                );
        }

        public async Task<VeterinariansModel?> GetVeterinariansByIdAsync(int id)
        {
            var Diagnostic = await _dataAccess.GetDataAsync<VeterinariansModel, dynamic>(
                 "dbo.spVeterinarians_GetById",
                 new { VeterinarianId = id }
             );

            return Diagnostic.FirstOrDefault();
        }
    }
}
