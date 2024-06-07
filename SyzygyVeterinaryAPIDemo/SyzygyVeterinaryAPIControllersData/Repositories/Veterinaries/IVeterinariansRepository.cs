using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Veterinaries
{
    public interface IVeterinariansRepository
    {
        Task AddVeterinariansAsync(VeterinariansModel veterinarians);
        Task DeleteVeterinariansAsync(int id);
        Task EditVeterinariansAsync(VeterinariansModel veterinarians);
        Task<IEnumerable<VeterinariansModel>> GetAllVeterinariansAsync();
        Task<VeterinariansModel?> GetVeterinariansByIdAsync(int id);
    }
}
