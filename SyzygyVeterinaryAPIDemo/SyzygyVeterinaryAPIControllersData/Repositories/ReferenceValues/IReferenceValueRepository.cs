using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyzygyVeterinaryAPIControllersData.Models;

namespace SyzygyVeterinaryAPIControllersData.Repositories.ReferenceValues
{
	public interface IReferenceValueRepository
	{
		Task AddReferenceValueAsync(ReferenceValueModel referenceValue);
		Task DeleteReferenceValueAsync(int id);
		Task EditReferenceValueAsync(ReferenceValueModel referenceValue);
		Task<IEnumerable<ReferenceValueModel>> GetAllReferenceValuesAsync();
		Task<ReferenceValueModel?> GetReferenceValueByIdAsync(int id);
	}
}
