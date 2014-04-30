/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobServe.API
{
	/// <summary>
	/// Interface for an object that makes asynchronous web requests and returns the results via strongly typed request/response
	/// objects (intended to be the API Types classes generated from the schema, in APITypes.cs).
	/// </summary>
	public interface IWebRequestManager
	{
		/// <summary>
		/// Returns a task that executes a get request to the specified API path, deserializing the 
		/// result as the given type <typeparamref name="TResponse"/>.
		/// 
		/// In it's current form, this method is not suitable for handling requests that return empty
		/// responses.  However, on the public API no such methods exist.
		/// </summary>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="relativePathAndQuery"></param>
		/// <param name="enableCompression"></param>
		/// <param name="secure"></param>
		/// <returns></returns>
		Task<TResponse> Get<TResponse>(string relativePathAndQuery, bool enableCompression = true, bool secure = false);
		/// <summary>
		/// Returns a task that executes a POST/PUT etc request with an entity body, deserializing
		/// the result as the given type <typeparamref name="TResponse"/>.
		/// 
		/// As with <see cref="Get"/>, this method is not suitable for handling requests that
		/// return empty responses.
		/// </summary>
		/// <typeparam name="TContent"></typeparam>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="relativePathAndQuery"></param>
		/// <param name="method"></param>
		/// <param name="content"></param>
		/// <param name="enableCompression"></param>
		/// <param name="secure"></param>
		/// <returns></returns>
		Task<TResponse> Send<TContent, TResponse>(string relativePathAndQuery, HttpMethod method, TContent content, bool enableCompression = true, bool secure = false);
	}
}
