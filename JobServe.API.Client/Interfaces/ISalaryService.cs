/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobServe.API
{
	/// <summary>
	/// Interface for a service providing Salary data exposed by the JobServe API.
	/// </summary>
	public interface ISalaryService : IRemoteCollectionService<SalaryBandsCollection>
	{
		/// <summary>
		/// Gets all the salary bands that match an optional country, optional currency and optional frequency.
		/// Note that parameters must be filled up from the left - so if you specify currency you must specify
		/// country.  Equally if you specify frequency, you must specify country and currency.
		/// </summary>
		/// <param name="country">ID of the country to be matched, if applicable</param>
		/// <param name="currency">ID of the currency to be matched, if applicable.  Please note, the JobServe API 
		/// exposes salary information for only a narrow set of currencies in certain coutnries.</param>
		/// <param name="frequency">ID of the payment frequency to be matched, if applicable.  You can get the
		/// list of frequencies supported with a call to <see cref="GetSalaryFrequencies"/></param>
		/// <returns></returns>
		Task<SalaryBands[]> GetSalaryBands(string country = null, string currency = null, string frequency = null);
		/// <summary>
		/// Returns all salary frequencies, or only those that match the IDs passed.  Any IDs that aren't
		/// matched yield a null result in the output enumerable.
		/// </summary>
		/// <returns></returns>
		Task<SalaryFrequency[]> GetSalaryFrequencies(IEnumerable<string> ids = null);
		/// <summary>
		/// Retrieves an individual salary frequency - or null if not found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<SalaryFrequency> GetSalaryFrequency(string id);
	}
}
