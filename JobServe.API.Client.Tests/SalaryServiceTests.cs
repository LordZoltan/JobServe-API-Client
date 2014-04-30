using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobServe.API.Tests
{
	[TestClass]
	public class SalaryServiceTests
	{
		ISalaryService GetService(IWebRequestManager requestManager = null)
		{
			return new SalaryService(new Client(requestManager ?? new WebRequestManager()));
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
			//ensures that the array when querying by IDs is identical to the Salary collection
			//that is loaded from the service
			var service = GetService();

			var items = await service.GetSalaryBands();
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

			var items = await service.GetSalaryBands();
			var collection = await service.GetUnderlyingCollection();
			//only one response should have been received.
			Assert.AreEqual(1, reqManager.ResponsesReceived);
		}

		private string FormatCurrency(Currency c, decimal value)
		{
			//beware - this method of formatting strings didn't work in the Xamarin stack the last time I tried it.
			return string.Format(GetCorrectNumberFormatFor(c), "{0:C}", value);
		}

		private NumberFormatInfo GetCorrectNumberFormatFor(Currency c)
		{
			//thanks to an answer by 'The Skeet' on SO which was actually showing how to eliminate 
			//currency symbol (use string.Empty) whilst preserving the rest of the formatting.
			NumberFormatInfo n = (NumberFormatInfo)CultureInfo.CurrentUICulture.NumberFormat.Clone();
			n.CurrencySymbol = c.Symbol;
			return n;
		}

		[TestMethod]
		public async Task ShouldGetSomeBands()
		{
			var service = GetService();
			//introduce a currency service here, using the same client that the salary service does
			var currencyService = new CurrencyService(service.Client);
			//going to get AUS/AUD bands here to demonstrate the effects of correct currency formatting
			//for the local culture, and I'm based in the UK.
			var bands = await service.GetSalaryBands("AUS", "AUD");
			var currency = await currencyService.GetCurrency("AUD");
			//technically, this test can, of course, fail because of the currency now - but if that's the 
			//case, then the currency tests will also be failing.  So I'm not going to lose any sleep over that.
			Assert.IsNotNull(currency);
			Assert.IsNotNull(bands);

			//note - in order to correctly format salary ranges for display, you need to get the current
			//UI culture, and produce a new number format from it's default, with the currency's currency symbol.
			var numberFormat = GetCorrectNumberFormatFor(currency);

			SalaryFrequency frequency;
			foreach (var band in bands)
			{
				frequency = await service.GetSalaryFrequency(band.Meta.Frequency);
				Console.WriteLine("AUS/AUD, Frequency: {0}", frequency.Text);
				foreach (var range in band.Ranges)
				{
					Console.WriteLine(range.Text);
				}
			}
		}
	}
}
