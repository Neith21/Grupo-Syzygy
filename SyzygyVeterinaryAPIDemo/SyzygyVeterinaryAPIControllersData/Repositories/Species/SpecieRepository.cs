using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.Species
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly IDbDataAccess _dataAccess;

        public SpeciesRepository(IDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<SpeciesModel>> GetAllSpeciesAsync()
        {
            var species = await _dataAccess.GetDataAsync<SpeciesModel, dynamic>(
                "dbo.sp_Species_GetAll",
                new { }
            );

            return species;
        }

        public async Task<SpeciesModel?> GetSpeciesByIdAsync(int id)
        {
            var species = await _dataAccess.GetDataAsync<SpeciesModel, dynamic>(
                "dbo.sp_Species_GetById",
                new { SpeciesId = id }
            );

            return species.FirstOrDefault();
        }

        public async Task AddSpeciesAsync(SpeciesModel species)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_Species_Insert",
                new { species.SpeciesName }
            );
        }

        public async Task EditSpeciesAsync(SpeciesModel species)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_Species_Update",
                new { species.SpeciesId, species.SpeciesName }
            );
        }

        public async Task DeleteSpeciesAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.sp_Species_Delete",
                new { SpeciesId = id }
            );
        }
    }
}
