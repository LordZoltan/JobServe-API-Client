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
	/// Interface for a service providing industry data exposed by the JobServe API.  These industries 
	/// are those that you can search by and find tagged to jobs.
	/// </summary>
	public interface IIndustryService : IRemoteCollectionService<IndustryCollection>
	{
		/// <summary>
		/// Returns a task that gets an array of industries - either all if no ids are passed, or
		/// only those whose IDs match those passed.
		/// 
		/// Note - you can also get the underlying collection through the IRemoteCollectionService interface's GetUnderlyingCollection method.
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		Task<Industry[]> GetIndustries(IEnumerable<string> ids = null);
		/// <summary>
		/// Retrieves the industry with the specified ID, or null if not found.  Note that this is 
		/// asynchronous to allow for situations where the underlying collection is to be downloaded
		/// from the API servers first
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Industry> GetIndustry(string id);
	}
}
