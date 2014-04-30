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
	using System.Diagnostics.Contracts;
	using APISalaryFrequencyCollection = JobServe.API.SalaryFrequencyCollection;
	public class SalaryService : RemoteCollectionService<SalaryMetadata, SalaryBands, SalaryBandsCollection>, ISalaryService
	{
		private class SalaryFrequencyCollection : RemoteCollectionService<SalaryFrequency, APISalaryFrequencyCollection>
		{
			public SalaryFrequencyCollection(IClient client, IWebServiceObjectCache cache = null)
				: base(client, cache)
			{

			}

			protected override Task<APISalaryFrequencyCollection> LoadAll(IClient client)
			{
				return client.GetSalaryFrequencies();
			}

			protected override string GetKeyFor(SalaryFrequency item)
			{
				return item.ID;
			}

			public Task<SalaryFrequency[]> GetSalaryFrequencies(IEnumerable<string> ids = null)
			{
				return GetItems(ids);
			}

			public Task<SalaryFrequency> GetSalaryFrequency(string id)
			{
				return GetItem(id);
			}
		}

		private readonly SalaryFrequencyCollection SalaryFrequencies;

		public SalaryService(IClient client, IWebServiceObjectCache cache = null) : base(client, cache)
		{
			client.ThrowIfNull("client");
			SalaryFrequencies = new SalaryFrequencyCollection(client, cache);
		}

		protected override Task<SalaryBandsCollection> LoadAll(IClient client)
		{
			return client.GetSalaryBands();
		}

		protected override SalaryMetadata GetKeyFor(SalaryBands item)
		{
			return item.Meta;
		}

		#region ISalaryService Members

		private static readonly SalaryBands[] EmptySalaryBands = new SalaryBands[0];

		public async Task<SalaryBands[]> GetSalaryBands(string country = null, string currency = null, string frequency = null)
		{
			if (country == null && currency == null && frequency == null)
				return await GetItems();
			SalaryMetadata meta = new SalaryMetadata() { Country = country, Currency = currency, Frequency = frequency };
			var matchingKeys = (await GetAllKeys()).Where(sm => sm.IsPartialMatch(meta)).ToArray();
			if (matchingKeys.Length == 0)
				return EmptySalaryBands;
			return await GetItems(matchingKeys);
		}

		public Task<SalaryFrequency[]> GetSalaryFrequencies(IEnumerable<string> ids = null)
		{
			return SalaryFrequencies.GetSalaryFrequencies(ids);
		}

		public Task<SalaryFrequency> GetSalaryFrequency(string id)
		{
			return SalaryFrequencies.GetSalaryFrequency(id);
		}

		#endregion
	}
}
