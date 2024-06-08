using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Animals
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public AnimalRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<AnimalModel>> GetAllAnimalsAsync()
        {
            var animals = await _dataAccess.GetDataForTwoForeignAsync<AnimalModel, AnimalOwnerModel, SpeciesModel, dynamic>(
                "dbo.spAnimals_GetAll",
                new { },
                (animal, animalOwner, specie) =>
                {
                    animal.AnimalOwners = animalOwner;
                    animal.Species = specie;
                    return animal;
                },
                splitOn: "AnimalOwnerId,SpeciesId"
            );

            return animals;
        }

        public async Task<AnimalModel?> GetAnimalsByIdAsync(int id)
        {
            var animals = await _dataAccess.GetDataAsync<AnimalModel, dynamic>(
                "dbo.spAnimals_GetById",
                new { AnimalId = id }
            );

            return animals.FirstOrDefault();
        }

        public async Task AddAnimalsAsync(AnimalModel animal)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spAnimals_Insert",
                new { animal.AnimalName, animal.AnimalAge, animal.AnimalGender, animal.AnimalOwnerId, animal.SpeciesId }
            );
        }

        public async Task EditAnimalsAsync(AnimalModel animal)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spAnimals_Update",
                new { animal.AnimalId, animal.AnimalName, animal.AnimalAge, animal.AnimalGender, animal.AnimalOwnerId, animal.SpeciesId }
            );
        }

        public async Task DeleteAnimalsAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spAnimals_Delete",
                new { AnimalId = id }
            );
        }
    }
}
