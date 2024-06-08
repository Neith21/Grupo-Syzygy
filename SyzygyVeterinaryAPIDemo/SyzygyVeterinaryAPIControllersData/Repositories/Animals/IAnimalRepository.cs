using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Animals
{
    public interface IAnimalRepository
    {
        Task AddAnimalsAsync(AnimalModel animal);
        Task DeleteAnimalsAsync(int id);
        Task EditAnimalsAsync(AnimalModel animal);
        Task<IEnumerable<AnimalModel>> GetAllAnimalsAsync();
        Task<AnimalModel?> GetAnimalsByIdAsync(int id);
    }
}