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
	public class IndustryService : RemoteCollectionService<Industry, IndustryCollection>, IIndustryService
	{
		public IndustryService(IClient client, IWebServiceObjectCache cache = null)
			: base(client, cache)
		{

		}

		protected override Task<IndustryCollection> LoadAll(IClient client)
		{
			return client.GetIndustries();
		}

		protected override string GetKeyFor(Industry item)
		{
			return item.ID;
		}

		#region IIndustryService Members

		public Task<Industry[]> GetIndustries(IEnumerable<string> ids = null)
		{
			return GetItems(ids);
		}

		public Task<Industry> GetIndustry(string id)
		{
			return GetItem(id);
		}

		#endregion
	}
}
