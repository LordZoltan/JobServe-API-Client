using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobServe.API.Tests
{
	[TestClass]
	public class WebRequestManagerTests
	{
		//this is by no means meant to be an exahustive test of all the features of the API,
		//just a couple of examples of how to use it.

		//to get the relative path of the API operation you want to call, just look at the documentation
		//for that operation at https://services.jobserve.com/Developers/Operations, and under the
		//'Full Uri and Verb' section you will see something like:
		//GET https://services.jobserve.com/[path]
		//Call it with this client, simply but the [path] bit in as the path argument to the client's Get
		//and Send methods.

		[TestMethod]
		public async Task ShouldGetVersion()
		{
			var client = new WebRequestManager();
			
			//best practise is to switch off compression for operations returning small amounts of data.
			var result = await client.Get<WebServiceVersionInfo>("version", enableCompression: false);
			Assert.IsNotNull(result);
			Console.WriteLine("Web service is v{0}, Date: {1}, Config Date: {2}", result.CodeVersion, result.CodeTimestamp, result.ConfigTimestamp);
		}

		[TestMethod]
		public async Task ShouldRunJobSearchForProgrammerJobs()
		{
			//this shows one way of doing this - get your default search first, then modify it
			//note that in production code you should get the default once, then clone it for re-use.
			//a simple way of doing that is to serialize it and deserialize it again using the 
			//DataContractSerializer (see the client's Send method and private Deserialize method)
			var client = new WebRequestManager();
			var defaultSearch = await client.Get<JobSearch>("jobsearchdefaults");

			/* location notes */

			//note that the service should return a default country of UK.
			//if you want to change the location, it's done via a collection.
			//Uncomment the below code to fix the location to 'london' in the UK

			//if(defaultSearch.Locations == null)
			//	defaultSearch.Locations = new LocationCollection();
			//else if(defaultSearch.Locations.Count != 0)
			//	defaultSearch.Locations.Clear();

			//defaultSearch.Locations.Add(new Location() { Country = "GBR", Text = "London"});

			//searching by location like this is the same as selecting a search country and then
			//entering a boolean query in the location field on the website.

			//you can also use the "Get Location Match" API (http://services.jobserve.com/Developers/Operations/GET/Locations/~latitude~/~longitude~)
			//to get a Location object, and add it to this collection.
 
			/* end of location notes */

			//alter the search skills to '.net AND developer'
			defaultSearch.Skills = ".net AND developer";

			//note - it's possible to send a search without getting the job content by doing this:
			// defaultSearch.JobIDsOnly = true;
			//however in this case we want the content at the same time.
			//Splitting the search and job content retrieval into two steps is used for more advanced 
			//searching interfaces - typically where some job content caching is being used.

			//get the first page of results (please note you can only request a maximum of 20 jobs per page)
			//note that we're using the HTTP POST flavour of the search API here - since our client can automatically
			//serialize a search to the entity body.  Whilst you *can* use the 
			var firstPage = await client.Send<JobSearch, JobSearchResults>("jobs", HttpMethod.Post, defaultSearch);

			Assert.IsNotNull(firstPage);
			//hopefully this test will always pass, or I'm out of a job!
			Assert.AreNotEqual(0, firstPage.JobCount);

			Console.WriteLine("Got {0} jobs for '.net AND developer'", firstPage.JobCount);
			Console.WriteLine();
			
			foreach (var job in firstPage.Jobs)
			{
				Console.WriteLine("{0} in {1} ({2})",
					job.Position,
					job.Location != null ? string.Format("{0}, {1}", job.Location.Text, job.Location.Country) : "(unknown)",
					job.Salary ?? "unknown salary");

				Console.WriteLine("Permalink: {0}", job.Permalink);
				Console.WriteLine();
			}
		}
	}
}
