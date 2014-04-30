using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobServe.API.Tests
{
	[TestClass]
	public class JobSearchServiceTests
	{
		private IJobSearchService GetService()
		{
			return new JobSearchService(new Client(new WebRequestManager()));
		}

		[TestMethod]
		public async Task ShouldGetSearchDefaults()
		{
			var service = GetService();
			var searchDefaults = await service.CreateDefaultSearch();
			Assert.IsNotNull(searchDefaults);
			Console.WriteLine(APITypes.Serialize(searchDefaults));
		}

		[TestMethod]
		public async Task DefaultSearchShouldReturnSomeResults()
		{
			//could be classed as a brittle test, but if JobServe doesn't return
			//any jobs for this default search, then JobServe probably doesn't
			//exist any more.

			var service = GetService();
			var searchDefaults = await service.CreateDefaultSearch();
			var results = await service.Search(searchDefaults);

			Assert.IsNotNull(results);
			Assert.AreNotEqual(0, results.JobCount);
			Console.WriteLine("Returned {0} job(s)", results.JobCount);
		}

		[TestMethod]
		public async Task ShouldGetJobsWithResults()
		{
			//you'll notice there is no JobService in this code at the moment - that's simply because it would
			//just be a wrapper for the client code unless you take it to the next logical step, which is to
			//implement some kind of job caching in that service to reduce the amount of data pulled from the 
			//Jobs API as you run searches (if you're using this client for personal use, it's likely you'll be
			//running searches that overlap results).

			//I don't have the time to show how that would be done at the moment, so in the meantime, you just
			//make sure the JobIDsOnly flag is set to false (it might already be in the default search tbh),
			//and you'll get the job data as expected.

			var service = GetService();
			var searchDefaults = await service.CreateDefaultSearch();
			//make sure this is set to false, and the Jobs API will give you the job data with each page of results,
			//not just the job IDs.  For piecemeal job-fetching, there is an operation on the Client class.
			searchDefaults.JobIDsOnly = false;
			var results = await service.Search(searchDefaults);

			Assert.IsNotNull(results);
			Assert.AreNotEqual(0, results.JobCount);
			Console.WriteLine("Returned {0} job(s)", results.JobCount);
			Assert.IsNotNull(results.Jobs);
			Assert.AreNotEqual(0, results.Jobs.Count);
			Assert.AreEqual(results.JobIDs[0], results.Jobs[0].ID);

			Console.WriteLine(APITypes.Serialize(results.Jobs[0]));

		}
	}
}
