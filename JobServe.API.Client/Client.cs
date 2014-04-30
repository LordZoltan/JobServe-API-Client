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
using System.Net.Http;
using System.Text;

namespace JobServe.API
{
	public class Client : IClient
	{
		private readonly IWebRequestManager _requestManager;
		public IWebRequestManager RequestManager { get { return _requestManager; } }

		public Client(IWebRequestManager requestManager)
		{
			requestManager.ThrowIfNull("requestManager");
			_requestManager = requestManager;
		}

		#region IClient Members

		//note on the methods below - compression is disabled in cases where the data is unlikely to be very big

		public System.Threading.Tasks.Task<WebServiceVersionInfo> GetCurrentVersion()
		{
			return _requestManager.Get<WebServiceVersionInfo>("Version", enableCompression: false);
		}

		public System.Threading.Tasks.Task<CountryCollection> GetCountries()
		{
			return _requestManager.Get<CountryCollection>("Countries", enableCompression: true);
		}

		public System.Threading.Tasks.Task<CurrencyCollection> GetCurrencies()
		{
			return _requestManager.Get<CurrencyCollection>("Currencies", enableCompression: true);
		}

		public System.Threading.Tasks.Task<IndustryCollection> GetIndustries()
		{
			return _requestManager.Get<IndustryCollection>("Industries", enableCompression: true);
		}

		public System.Threading.Tasks.Task<JobTypeCollection> GetJobTypes()
		{
			return _requestManager.Get<JobTypeCollection>("JobTypes", enableCompression: false);
		}

		public System.Threading.Tasks.Task<JobSearch> GetSearchDefaults()
		{
			return _requestManager.Get<JobSearch>("JobSearchDefaults", enableCompression: false);
		}

		public System.Threading.Tasks.Task<ValueListsIndex> GetSearchValueLists()
		{
			return _requestManager.Get<ValueListsIndex>("JobSearchValueLists", enableCompression:true);
		}

		public System.Threading.Tasks.Task<SalaryFrequencyCollection> GetSalaryFrequencies()
		{
			return _requestManager.Get<SalaryFrequencyCollection>("Salaries/Frequencies", enableCompression: false);
		}

		public System.Threading.Tasks.Task<SalaryBandsCollection> GetSalaryBands(string country = null, string currency = null, string frequency = null)
		{
			if (string.IsNullOrWhiteSpace(country) && string.IsNullOrWhiteSpace(currency) && string.IsNullOrWhiteSpace(frequency))
				return _requestManager.Get<SalaryBandsCollection>("Salaries", enableCompression: true);
			else
			{
				if(!string.IsNullOrWhiteSpace(currency) && string.IsNullOrWhiteSpace(country))
					throw new ArgumentException("country must be provided if currency is provided.", "country");
				if(!string.IsNullOrWhiteSpace(frequency) && string.IsNullOrWhiteSpace(currency))
					throw new ArgumentException("currency must be provided if frequency is provided", "currency");

				StringBuilder relativeUri = new StringBuilder("Salaries");
				if (!string.IsNullOrWhiteSpace(country))
				{
					relativeUri.Append("/");
					relativeUri.Append(country);
					if (!string.IsNullOrWhiteSpace(currency))
					{
						relativeUri.Append("/");
						relativeUri.Append(currency);
						if (!string.IsNullOrWhiteSpace(frequency))
						{
							relativeUri.Append("/");
							relativeUri.Append(frequency);
						}
					}
				}
				return _requestManager.Get<SalaryBandsCollection>(relativeUri.ToString(), enableCompression: true);
			}
		}

		public System.Threading.Tasks.Task<GeoLocationMatch> GeoLocate(double latitude, double longitude, double? maxDistance = null)
		{
			string relativeUri = string.Format("Locations/{0}/{1}", latitude, longitude);
			if ((maxDistance ?? 0) != 0)
				relativeUri = relativeUri + string.Format("?maxDistance={0}", maxDistance);
			return _requestManager.Get<GeoLocationMatch>(relativeUri, enableCompression: false);
		}

		public System.Threading.Tasks.Task<JobSearchResults> Search(JobSearch search)
		{
			search.ThrowIfNull("search");
			return _requestManager.Send<JobSearch, JobSearchResults>("Jobs", HttpMethod.Post, search, enableCompression: true);
		}

		public System.Threading.Tasks.Task<Job> GetJob(string id)
		{
			id.ThrowIfIsNullOrWhiteSpace("id");
			//please note - if the job is no longer available, the task will fail with an exception,
			//raised by an underlying 404 status code.
			return _requestManager.Get<Job>("Jobs/" + id, enableCompression: true);
		}

		public System.Threading.Tasks.Task<JobCollection> GetJobs(IEnumerable<string> jobIDs)
		{
			jobIDs.ThrowIfIsNullOrEmptyOrAny(id => string.IsNullOrWhiteSpace(id), "IDs must be non-null, non-empty, and no IDs can be null, whitespace or empty", "jobIDs");
			return _requestManager.Send<IDCollection, JobCollection>("Jobs/List", HttpMethod.Post, new IDCollection(), enableCompression: true);
		}

		#endregion
	}
}
