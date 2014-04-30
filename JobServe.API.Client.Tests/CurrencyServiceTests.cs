using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobServe.API.Tests
{
	[TestClass]
	public class CurrencyServiceTests
	{
		ICurrencyService GetService(IWebRequestManager requestManager = null)
		{
			return new CurrencyService(new Client(requestManager ?? new WebRequestManager()));
		}

		[TestMethod]
		public async Task ShouldGetUnderlyingCollection()
		{
			//get the underlying collection that's returned from the jobs API.  behind the scenes
			//this will load it from the client first, cache it and return it.
			var collection = await GetService().GetUnderlyingCollection();
			Assert.IsNotNull(collection);
			Assert.AreNotEqual(0, collection.Count);
		}

		[TestMethod]
		public async Task ShouldGetAllItems()
		{
			//ensures that the array when querying by IDs is identical to the Currency collection
			//that is loaded from the service
			var service = GetService();

			var items = await service.GetCurrencies();
			var collection = await service.GetUnderlyingCollection();
			Assert.IsNotNull(items);
			Assert.IsTrue(collection.SequenceEqual(items));
		}

		[TestMethod]
		public async Task ShouldOnlyMakeOneRequest()
		{
			//gets the same data as above, but checks that the caching is working,
			//which is, after all, the point of having the service in the first place.
			CountingRequestManager reqManager = new CountingRequestManager();

			var service = GetService(reqManager);

			var items = await service.GetCurrencies();
			var collection = await service.GetUnderlyingCollection();
			//only one response should have been received.
			Assert.AreEqual(1, reqManager.ResponsesReceived);
		}

		[TestMethod]
		public async Task ShouldGetAnItem()
		{
			var service = GetService();
			//just get a known Currency
			var item = await service.GetCurrency("GBP");

			Assert.IsNotNull(item);
		}
	}
}
