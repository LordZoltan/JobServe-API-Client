using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobServe.API;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JobServe.API.Tests
{
	[TestClass]
	public class ClientTests
	{
		private IClient GetClient()
		{
			//clearly, for your code you would pass in your token
			return new Client(new WebRequestManager());
		}

		[TestMethod]
		public async Task ShouldGetVersion()
		{
			var client = GetClient();

			var result = await client.GetCurrentVersion();
			Assert.IsNotNull(result);
			Console.WriteLine("Code Version: {0}, Build Date: {1}, Config Date: {2}", result.CodeVersion, result.CodeTimestamp, result.ConfigTimestamp);
		}

		[TestMethod]
		public async Task ShouldGetCountries()
		{
			var client = GetClient();
			CountryCollection countries = await client.GetCountries();
			Assert.IsNotNull(countries);
			Assert.AreNotEqual(0, countries.Count);
			foreach (var country in countries)
			{
				Console.WriteLine("Country ID: {0}, Display Name: {1}, Native Name: {2}", country.ID, country.DisplayName, country.NativeName);

			}
		}

		[TestMethod]
		public async Task ShouldGetIndustries()
		{
			var client = GetClient();
			IndustryCollection industries = await client.GetIndustries();
			Assert.IsNotNull(industries);
			Assert.AreNotEqual(0, industries.Count);
			foreach (var industry in industries)
			{
				Console.WriteLine("Industry ID: {0}, Display Name: {1}", industry.ID, industry.DisplayName);
			}
		}

		[TestMethod]
		public async Task ShouldGetJobTypes()
		{
			var client = GetClient();
			JobTypeCollection jobTypes = await client.GetJobTypes();
			Assert.IsNotNull(jobTypes);
			Assert.AreNotEqual(0, jobTypes.Count);
			foreach (var jobType in jobTypes)
			{
				Console.WriteLine("Job Type ID: {0}, Description: {1}", jobType.ID, jobType.Text);
			}
		}

		[TestMethod]
		public async Task ShouldGetJobSearchDefaults()
		{
			var client = GetClient();
			JobSearch jobSearchDefaults = await client.GetSearchDefaults();
			Assert.IsNotNull(jobSearchDefaults);
		}

		[TestMethod]
		public async Task ShouldGetCanaryWharf()
		{
			var client = GetClient();
			GeoLocationMatch canaryWharf = await client.GeoLocate(51.50442, -0.01835);
			Assert.IsNotNull(canaryWharf);
			Console.WriteLine("Distance (m): {0}, ID: {1}, Latitude: {2}, Longitude: {3}, Description: {4}, Country: {5}", canaryWharf.Distance, canaryWharf.Location.ID, canaryWharf.Location.Latitude, canaryWharf.Location.Longitude, canaryWharf.Location.Text, canaryWharf.Location.Country);
		}

		private string FormatListItem(ValueListItem item)
		{
			if (item.Value != null && item.Description != null)
				return string.Format("Value: {0}, Description: {1}", item.Value, item.Description);
			else if (item.Value != null)
				return string.Format("Value: {0}", item.Value);
			return "";
		}

		[TestMethod]
		public async Task ShouldGetJobSearchValueLists()
		{
			//BEWARE - thanks to bug 11881 in Xamarin's framework
			//(https://bugzilla.xamarin.com/show_bug.cgi?id=11881)
			//this code will fail on Xamarin.Android.  There is a workaround,
			//but it requires a different version of the type to be built using
			//a set of convoluted generics that serialize and deserialize like
			//a dictionary.

			var client = GetClient();
			ValueListsIndex valueLists = await client.GetSearchValueLists();
			Assert.IsNotNull(valueLists);
			Assert.AreNotEqual(0, valueLists.Count);

			foreach (var list in valueLists)
			{
				Console.WriteLine("---- Bound member: {0} ----", list.Key);
				if (list.Value.DefaultValue != null)
					Console.WriteLine("Default Value: {0}", FormatListItem(list.Value.DefaultValue));
				if (list.Value.EmptyValue != null)
					Console.WriteLine("Empty Value: {0}", FormatListItem(list.Value.EmptyValue));
				Console.WriteLine("Items:");
				foreach (var item in list.Value.Items)
				{
					Console.WriteLine(FormatListItem(item));
				}
			}
		}

		[TestMethod]
		public async Task ShouldGetSalaryFrequencies()
		{
			var client = GetClient();
			SalaryFrequencyCollection frequencies = await client.GetSalaryFrequencies();
			Assert.IsNotNull(frequencies);
			Assert.AreNotEqual(0, frequencies.Count);
			foreach (var frequency in frequencies)
			{
				Console.WriteLine("{0}: {1}. Format: {2}. Range format: {3}, Range format (no lower): {4}. Range format (no upper): {5}. ", frequency.ID, frequency.Text, frequency.SalaryFormat, frequency.SalaryRangeFormat, frequency.SalaryRangeFormatNoLower, frequency.SalaryRangeFormatNoUpper);
			}
		}

		[TestMethod]
		public async Task ShouldGetSalaryBands()
		{
			var client = GetClient();
			//get the frequencies as well - so we can format them
			SalaryFrequencyCollection frequencies = await client.GetSalaryFrequencies();
			Assert.IsNotNull(frequencies);
			Assert.AreNotEqual(0, frequencies.Count);
			Dictionary<string, SalaryFrequency> freqLookup = frequencies.ToDictionary(sf => sf.ID);
			SalaryBandsCollection bands = await client.GetSalaryBands();
			string msg;
			foreach (var band in bands)
			{
				Console.WriteLine("Band for Country {0}, Currency {1}, Frequency {2}", band.Meta.Country, band.Meta.Currency, band.Meta.Frequency);
				foreach (var range in band.Ranges)
				{
					msg = null;
					if (range.Lower == null)
					{
						if (range.Upper != null)
							msg = string.Format(freqLookup[band.Meta.Frequency].SalaryRangeFormatNoLower, range.Upper.Value ?? 0);
					}
					else if (range.Upper == null)
					{
						if (range.Lower != null)
							msg = string.Format(freqLookup[band.Meta.Frequency].SalaryRangeFormatNoUpper, range.Lower.Value ?? 0);
					}
					else
						msg = string.Format(freqLookup[band.Meta.Frequency].SalaryRangeFormat, range.Lower.Value ?? 0, range.Upper.Value ?? 0);

					if (msg != null)
						Console.WriteLine(msg);
				}

			}
		}

		private TJobSearch GetCSharpLondonSearch<TJobSearch>() where TJobSearch : JobSearch, new()
		{

			TJobSearch s = new TJobSearch()
			{
				Industries = new IDCollection() { "01" },
				JobIDsOnly = false,
				JobTypes = new IDCollection() { "Any" },
				Locations = new LocationCollection() { new Location() { Country = "GBR", Text = "London" } },
				MaxAge = 7,
				MaxDistance = 25,
				Page = 1,
				PageSize = 20,
				Skills = "C#",
				SortOrder = "EXPLORER_DATE_DESC"
			};
			return s;
		}

		[TestMethod]
		public async Task ShouldRunASearch()
		{
			var c = GetClient();
			JobSearch s = GetCSharpLondonSearch<JobSearch>();

			var results = await c.Search(s);
			Assert.IsNotNull(results);
			Console.WriteLine("No of results: {0}.  IDs from this page: {1}", results.JobCount, string.Join(", ", results.JobIDs));
		}

		[TestMethod]
		public async Task ShouldGetAJob()
		{
			//run a search with jobs switched on - get the first job that's returned and then compare IDs, permanent IDs and permalinks.
			var c = GetClient();
			JobSearch s = GetCSharpLondonSearch<JobSearch>();

			var results = await c.Search(s);
			Assert.IsNotNull(results);
			Assert.AreNotEqual(0, results.JobCount);

			var job = await c.GetJob(results.JobIDs[0]);
			Assert.IsNotNull(job);
			Assert.AreEqual(results.Jobs[0].ID, job.ID);
			Assert.AreEqual(results.Jobs[0].PermanentID, job.PermanentID);
			Console.WriteLine("Title: {0}, ID: {1}, Permanent ID: {2}, Permalink: {3}", job.Position, job.ID, job.PermanentID, job.Permalink);
		}
	}
}
