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
	/// <summary>
	/// Implementation of the IJobSearchValueListsService.  Lazy-loads the underlying data
	/// from the Jobs API, and then operates on a cached version for the lifetime of the object.
	/// </summary>
	public class JobSearchValueListsService : IJobSearchValueListsService
	{
		private readonly IClient Client;
		private readonly IWebServiceObjectCache Cache;

		private Lazy<Task<ValueListsIndex>> _index;

		/// <summary>
		/// Initializes a new instance of the <see cref="JobSearchValueListsService"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="cache">The cache.</param>
		public JobSearchValueListsService(IClient client, IWebServiceObjectCache cache = null)
		{
			client.ThrowIfNull("client");
			Client = client;
			Cache = cache;
			_index = new Lazy<Task<ValueListsIndex>>(() => LoadIndex());
		}

		private Task<ValueListsIndex> LoadIndex()
		{
			if (Cache != null) 
				return Cache.GetObject(c => c.GetSearchValueLists());
			else
				return Client.GetSearchValueLists();
		}

		/// <summary>
		/// Gets a particular value list for a specific named member of the JobSearch type.
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public async Task<ValueList> GetList(string memberName)
		{
			memberName.ThrowIfIsNullOrWhiteSpace("memberName");
			ValueList toReturn;
			(await _index.Value).TryGetValue(memberName, out toReturn);
			return toReturn;
		}

		/// <summary>
		/// Gets the entire index of value lists, so you can enumerate all the members and the associated lists.
		/// </summary>
		/// <returns></returns>
		public Task<ValueListsIndex> GetIndex()
		{
			return _index.Value;
		}
	}
}
