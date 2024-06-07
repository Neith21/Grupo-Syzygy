using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyzygyVeterinaryAPIControllersData.Data
{
	public interface IDbDataAccess
	{
		Task<IEnumerable<T>> GetDataAsync<T, P>(string storedProcedure, P parameters, string connection = "default");
		Task<IEnumerable<T>> GetDataForOneForeignAsync<T, U, P>(string storedProcedure, P parameters, Func<T, U, T>? map = null, string connection = "default", string splitOn = "Id");
		Task<IEnumerable<T>> GetDataForThreeForeignAsync<T, U, V, W, P>(string storedProcedure, P parameters, Func<T, U, V, W, T>? map = null, string connection = "default", string splitOn = "Id");
		Task<IEnumerable<T>> GetDataForTwoForeignAsync<T, U, V, P>(string storedProcedure, P parameters, Func<T, U, V, T>? map = null, string connection = "default", string splitOn = "Id");
		Task SaveDataAsync<T>(string storedProcedure, T parameters, string connection = "default");
	}
}
