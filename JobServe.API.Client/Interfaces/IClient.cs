/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobServe.API
{
	/// <summary>
	/// Asynchronous JobServe API client providing explicit methods for performing typical API requests.
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// The request manager that this client uses to make all its requests.
		/// </summary>
		IWebRequestManager RequestManager { get; }

		/// <summary>
		/// Gets the current version of the API from the servers.
		/// </summary>
		/// <returns></returns>
		Task<WebServiceVersionInfo> GetCurrentVersion();

		/// <summary>
		/// Gets the countries supported by the API.
		/// </summary>
		/// <returns></returns>
		Task<CountryCollection> GetCountries();

		/// <summary>
		/// Gets the currencies supported by the API.
		/// </summary>
		/// <returns></returns>
		Task<CurrencyCollection> GetCurrencies();

		/// <summary>
		/// Gets the industries supported by the API.
		/// </summary>
		/// <returns></returns>
		Task<IndustryCollection> GetIndustries();

		/// <summary>
		/// Gets the job types supported by the API (for searching and that will be found on jobs themselves).
		/// </summary>
		/// <returns></returns>
		Task<JobTypeCollection> GetJobTypes();

		/// <summary>
		/// Gets the default search you should use as the starting point for your searches.
		/// </summary>
		/// <returns></returns>
		Task<JobSearch> GetSearchDefaults();

		/// <summary>
		/// Gets the search value lists (for binding UI to some of the properties of a Job search).
		/// </summary>
		/// <returns></returns>
		Task<ValueListsIndex> GetSearchValueLists();

		/// <summary>
		/// Gets the salary frequencies supported by the API.
		/// </summary>
		/// <returns></returns>
		Task<SalaryFrequencyCollection> GetSalaryFrequencies();

		/// <summary>
		/// Gets the salary bands supported by the API.
		/// </summary>
		/// <param name="country">The country.</param>
		/// <param name="currency">The currency.</param>
		/// <param name="frequency">The frequency.</param>
		/// <returns></returns>
		Task<SalaryBandsCollection> GetSalaryBands(string country = null, string currency = null, string frequency = null);

		/// <summary>
		/// Attempts to find a location match near the passed latitude/longitude, optionally within the given distance (in metres).
		/// 
		/// It's very important to note that the matched location comes from JobServe's own database geared around locating
		/// jobs; not people.  Therefore the data cannot be used as a general purpose Geo-location API.
		/// Job location data is never fine-grained, for obvious reasons, but it's certainly accurate enough to be able to 
		/// provide jobs that are closest to where people are.
		/// </summary>
		/// <param name="latitude"></param>
		/// <param name="longitude"></param>
		/// <param name="maxDistance"></param>
		/// <returns></returns>
		Task<GeoLocationMatch> GeoLocate(double latitude, double longitude, double? maxDistance = null);

		/// <summary>
		/// Kinda obvious, surely...  
		/// </summary>
		/// <param name="search">The search.</param>
		/// <returns></returns>
		Task<JobSearchResults> Search(JobSearch search);

		/// <summary>
		/// Gets the details for an individual job
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Job> GetJob(string id);

		/// <summary>
		/// Used to retrieve multiple jobs in one go.
		/// </summary>
		/// <param name="jobIDs"></param>
		/// <returns></returns>
		Task<JobCollection> GetJobs(IEnumerable<string> jobIDs);
	}
}
