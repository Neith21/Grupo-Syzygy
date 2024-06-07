using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.ReferenceValues
{
	public class ReferenceValueRepository : IReferenceValueRepository
	{
		private readonly IDbDataAccess _dataAccess;

		public ReferenceValueRepository(IDbDataAccess dataAccess)
		{
			_dataAccess = dataAccess;
		}

		public async Task<IEnumerable<ReferenceValueModel>> GetAllReferenceValuesAsync()
		{
			var referenceValues = await _dataAccess.GetDataForOneForeignAsync<ReferenceValueModel, SpeciesModel, dynamic>(
				"dbo.spReferenceValues_GetAll",
				new { },
				(referenceValue, species) =>
				{
					referenceValue.Species = species;
					return referenceValue;
				},
				splitOn: "SpeciesId"
			);

			return referenceValues;
		}

		public async Task<ReferenceValueModel?> GetReferenceValueByIdAsync(int id)
		{
			var referenceValue = await _dataAccess.GetDataAsync<ReferenceValueModel, dynamic>(
				"dbo.spReferenceValues_GetById",
				new { ReferenceValueId = id }
			);

			return referenceValue.FirstOrDefault();
		}

		public async Task AddReferenceValueAsync(ReferenceValueModel referenceValue)
		{
			await _dataAccess.SaveDataAsync(
				"dbo.spReferenceValues_Insert",
				new { referenceValue.AgeRange, referenceValue.AnalysisType, referenceValue.ReferenceData, referenceValue.SpeciesId }
			);
		}

		public async Task EditReferenceValueAsync(ReferenceValueModel referenceValue)
		{
			await _dataAccess.SaveDataAsync(
				"dbo.spReferenceValues_Update",
				new { referenceValue.ReferenceValueId, referenceValue.AgeRange, referenceValue.AnalysisType, referenceValue.ReferenceData, referenceValue.SpeciesId }
			);
		}

		public async Task DeleteReferenceValueAsync(int id)
		{
			await _dataAccess.SaveDataAsync(
				"dbo.spReferenceValues_Delete",
				new { ReferenceValueId = id }
			);
		}
	}
}
