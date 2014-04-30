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
	/// Standard implementation of the ICountryService.
	/// </summary>
	public class CountryService : RemoteCollectionService<Country, CountryCollection>, ICountryService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CountryService"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="cache">The cache.</param>
		public CountryService(IClient client, IWebServiceObjectCache cache = null) : base(client, cache)
		{
		}

		/// <summary>
		/// Loads all.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <returns></returns>
		protected override Task<CountryCollection> LoadAll(IClient client)
		{
			return client.GetCountries();
		}

		protected override string GetKeyFor(Country item)
		{
			return item.ID;
		}

		#region ICountryService Members

		public Task<Country[]> GetCountries(IEnumerable<string> ids = null)
		{
			return GetItems(ids);
		}

		public Task<Country> GetCountry(string id)
		{
			return GetItem(id);
		}

		#endregion
	}
}
