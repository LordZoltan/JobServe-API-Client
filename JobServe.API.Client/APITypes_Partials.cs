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
	/// The SalaryMetadata type - base code is obtained from the API Code samples generated code,
	/// and then extended in APITypes_Partials.cs to add IEquatable
	/// </summary>
	public partial class SalaryMetadata : IEquatable<SalaryMetadata>
	{
		public override bool Equals(object obj)
		{
			return Equals(obj as SalaryMetadata);
		}
		public override int GetHashCode()
		{
			return (Country != null ? Country.GetHashCode() : 0) ^
				(Currency != null ? Currency.GetHashCode() : 0) ^
				(Frequency != null ? Frequency.GetHashCode() : 0);
		}

		#region IEquatable<SalaryMetadata> Members

		public bool Equals(SalaryMetadata other)
		{
			if (other == null) return false;
			return Country == other.Country && Currency == other.Currency && Frequency == other.Frequency;
		}

		#endregion

		/// <summary>
		/// Returns true if this key partially or wholly matches the passed filter.
		/// 
		/// When a filter's property (Country, Currency, Frequency) is null, then 
		/// it always matches the same property.  Thus a filter with all-null 
		/// properties will match any other.
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public bool IsPartialMatch(SalaryMetadata filter)
		{
			return (filter.Country == null || Country == filter.Country)
				&& (filter.Currency == null || Currency == filter.Currency)
				&& (filter.Frequency == null || Frequency == filter.Frequency);
		}
	}
}
