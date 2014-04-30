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
	/// Interface for a cache of objects (typically the static data, such as Countries etc) retrieved from the API.
	/// </summary>
	/// <remarks>
	/// In-memory caching is largely redundant if you use the service interfaces and their default implementations, 
	/// since that is what they do already.  This interface is provided for disk-based caches to preserve the static
	/// data between application restarts (so long as the Web API version remains the same also.  
	/// 
	/// Do NOT cache Job data on disk - this is a direct contravention of the APIs terms of use.
	/// 
	/// No implementation is currently provided for this interface.  If you do implement this interface, be sure
	/// to take culture (e.g. en-GB/en-US) into account; if this can change between restarts in your application, then
	/// be aware that some text exposed by the API is presented differently based on culture, even if the underlying
	/// values are identical.
	/// </remarks>
	public interface IWebServiceObjectCache
	{
		/// <summary>
		/// Retrieves the data identified by the given from the key, either from the cache (if it's still valid,
		/// or by pulling a new instance obtained from the client via the supplied delegate through the 
		/// cache on-demand.
		/// </summary>
		/// <typeparam name="T">Type of object to be retrieved.</typeparam>
		/// <param name="getObject">Delegate to be used to retrieve a new object from the client.</param>
		/// <param name="id">Unique ID of the object that the object is to be associated with in the cache.</param>
		/// <returns>A task that returns the object directly from the cache, possibly after a new instance
		/// is retrieved from the client first.</returns>
		Task<T> GetObject<T>(Func<IClient, Task<T>> getObject, string id = null) where T : class;
	}
}
