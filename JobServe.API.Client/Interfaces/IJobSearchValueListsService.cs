/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Threading.Tasks;
namespace JobServe.API
{
	/// <summary>
	/// Interface for a service that provides access to the value lists that are appropriate
	/// for certain members of the JobSearch object.
	/// </summary>
	public interface IJobSearchValueListsService
	{
		/// <summary>
		/// Gets the entire index of value lists, so you can enumerate all the members and the associated lists.
		/// </summary>
		/// <returns></returns>
		Task<ValueListsIndex> GetIndex();
		/// <summary>
		/// Gets a particular value list for a specific named member of the JobSearch type.
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		Task<ValueList> GetList(string memberName);
	}
}
