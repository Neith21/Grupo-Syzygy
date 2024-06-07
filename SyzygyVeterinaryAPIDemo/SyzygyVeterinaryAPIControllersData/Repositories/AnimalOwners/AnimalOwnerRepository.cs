using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.AnimalOwners
{
    public class AnimalOwnerRepository : IAnimalOwnerRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public AnimalOwnerRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<AnimalOwnerModel>> GetAllAnimalOwnersAsync()
        {
            var animalOwners = await _dataAccess.GetDataAsync<AnimalOwnerModel, dynamic>(
                "dbo.sp_AnimalOwner_GetAll",
                new { }
            );

            return animalOwners;
        }

        public async Task<AnimalOwnerModel?> GetAnimalOwnerByIdAsync(int id)
        {
            var animalOwner = await _dataAccess.GetDataAsync<AnimalOwnerModel, dynamic>(
                "dbo.sp_AnimalOwner_GetById",
                new { AnimalOwnerId = id }
            );

            return animalOwner.FirstOrDefault();
        }

        public async Task AddAnimalOwnerAsync(AnimalOwnerModel animalOwner)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_AnimalOwner_Insert",
                new { animalOwner.AnimalOwnerName, animalOwner.AnimalOwnerContactInfo }
            );
        }

        public async Task EditAnimalOwnerAsync(AnimalOwnerModel animalOwner)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_AnimalOwner_Update",
                new { animalOwner.AnimalOwnerId, animalOwner.AnimalOwnerName, animalOwner.AnimalOwnerContactInfo }
            );
        }

        public async Task DeleteAnimalOwnerAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_AnimalOwner_Delete",
                new { AnimalOwnerId = id }
            );
        }
    }
}
