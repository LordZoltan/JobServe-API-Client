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
	/// Generic base interface for a service that is built around one of the collection types
	/// retrieved from the API.
	/// </summary>
	/// <typeparam name="TCollection"></typeparam>
	public interface IRemoteCollectionService<TCollection> : IClientService
	{
		/// <summary>
		/// For services that simply wrap an instance of a strongly-typed collection that is returned
		/// by one operation on the web API; this operation provides the caller access to that collection.
		/// 
		/// E.g. for countries it might be the raw CountryCollection.  The method is asynchronous to
		/// allow for the possibility that the data has not yet been downloaded from the API when called.
		/// </summary>
		/// <returns></returns>
		Task<TCollection> GetUnderlyingCollection();
	}
}
