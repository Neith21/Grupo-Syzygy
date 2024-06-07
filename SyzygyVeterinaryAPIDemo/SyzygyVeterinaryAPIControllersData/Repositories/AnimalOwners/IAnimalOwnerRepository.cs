using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.AnimalOwners
{
    public interface IAnimalOwnerRepository
    {
        Task AddAnimalOwnerAsync(AnimalOwnerModel animalOwner);
        Task DeleteAnimalOwnerAsync(int id);
        Task EditAnimalOwnerAsync(AnimalOwnerModel animalOwner);
        Task<IEnumerable<AnimalOwnerModel>> GetAllAnimalOwnersAsync();
        Task<AnimalOwnerModel?> GetAnimalOwnerByIdAsync(int id);
    }
}