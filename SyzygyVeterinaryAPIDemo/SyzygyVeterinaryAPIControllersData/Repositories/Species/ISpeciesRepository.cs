using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Species
{
    public interface ISpeciesRepository
    {
        Task AddSpeciesAsync(SpeciesModel species);
        Task DeleteSpeciesAsync(int id);
        Task EditSpeciesAsync(SpeciesModel species);
        Task<IEnumerable<SpeciesModel>> GetAllSpeciesAsync();
        Task<SpeciesModel?> GetSpeciesByIdAsync(int id);
    }
}