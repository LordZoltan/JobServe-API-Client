/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobServe.API
{
	public class JobSearchService : IJobSearchService
	{
		private readonly IClient _client;
		private readonly Lazy<Task<JobSearch>> ServerDefaultSearch;

		public IClient Client { get { return _client; } }

		public JobSearchService(IClient client)
		{
			client.ThrowIfNull("client");
			_client = client;
			ServerDefaultSearch = new Lazy<Task<JobSearch>>(() => GetDefaultSearchFromServer());
		}

		protected async Task<JobSearch> GetDefaultSearchFromServer()
		{
			return await _client.GetSearchDefaults();
		}

		#region IJobSearchService Members

		public virtual async Task<JobSearchResults> Search(JobSearch search)
		{
			search.ThrowIfNull("search");
			return await _client.Search(search);
		}

		public virtual async Task<JobSearch> CreateDefaultSearch()
		{
			return APITypes.CloneAPIObject((await ServerDefaultSearch.Value));
		}

		#endregion
	}
}
