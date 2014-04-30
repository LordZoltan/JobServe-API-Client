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
	/// Simple service offering a 
	/// </summary>
	public interface IJobSearchService : IClientService
	{
		/// <summary>
		/// Runs the provided search, applying one or two ambient defaults along the way if they're not already set.
		/// Specifically the search location is set to the user's preferred location if it's null on the input search.
		/// 
		/// It's important to note that all the defaults from CreateDefeaultSearch are NOT applied to the input
		/// search; so any required members for search objects must be set, otherwise an error will occur.
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		Task<JobSearchResults> Search(JobSearch search);
		/// <summary>
		/// Gets the default job search as returned by the web servers - note, however, that any location on that 
		/// search is stripped off so that the default behaviour when searching can be to use the user's current/preferred location.
		/// 
		/// Note that the method is asynchronous because the core default values might need to be downloaded
		/// from the web servers before being customised locally.
		/// </summary>
		/// <returns></returns>
		Task<JobSearch> CreateDefaultSearch();
	}
}
