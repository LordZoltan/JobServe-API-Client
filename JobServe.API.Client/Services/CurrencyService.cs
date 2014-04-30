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
	public class CurrencyService : RemoteCollectionService<string, Currency, CurrencyCollection>, ICurrencyService
	{

		public CurrencyService(IClient client, IWebServiceObjectCache cache = null) : base(client, cache)
		{

		}

		protected override Task<CurrencyCollection> LoadAll(IClient client)
		{
			return client.GetCurrencies();
		}

		protected override string GetKeyFor(Currency item)
		{
			return item.ID;
		}

		#region ICurrencyService Members

		public Task<Currency[]> GetCurrencies(IEnumerable<string> ids = null)
		{
			return GetItems(ids);
		}

		public Task<Currency> GetCurrency(string id)
		{
			return GetItem(id);
		}

		#endregion

	}
}
