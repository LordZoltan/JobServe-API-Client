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

namespace JobServe.API
{
	/// <summary>
	/// Base interface for a service based on the API client.
	/// 
	/// Exposing the client as a property on all services allows new service instances
	/// to inherit a single client from other service instances.
	/// </summary>
	public interface IClientService
	{
		IClient Client { get; }
	}
}
