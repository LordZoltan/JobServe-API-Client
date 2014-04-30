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
	public class JobTypeService : RemoteCollectionService<JobType, JobTypeCollection>, IJobTypeService
	{	
		public JobTypeService(IClient client, IWebServiceObjectCache cache = null) : base(client, cache)
		{

		}

		protected override Task<JobTypeCollection> LoadAll(IClient client)
		{
			return client.GetJobTypes();
		}

		protected override string GetKeyFor(JobType item)
		{
			return item.ID;
		}

		#region IJobTypeService Members

		public Task<JobType[]> GetJobTypes(IEnumerable<string> ids = null)
		{
			return GetItems(ids);
		}

		public Task<JobType> GetJobType(string id)
		{
			return GetItem(id);
		}

		#endregion


	}
}
