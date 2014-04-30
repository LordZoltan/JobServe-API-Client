/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JobServe.API
{
	// Please Note: If you define the __ANDROID__ symbol to replace the stock ValueListsIndex type, then the resulting PCL library 
	// will only really be suitable for use with Xamarin.Android, despite being a PCL.  I've avoided doing a shared project 
	// specifically for that platform because the majority of clients will be Microsoft-flavoured Net and don't need this workaround,
	// and it also kinda goes against the philosophy of having a PCL in the first place.

#if __ANDROID__
	/// <summary>
	/// Replaces the default ValueListsIndex class that is obtained from the API website.  In order for the project to compile
	/// you must open that one (APITypes.cs) and surround the default ValueListsIndex class with an #if !__ANDROID__ block.
	/// In this GitHub check-in, I've already done that for the current version of the APITypes.
	/// </summary>
	[CollectionDataContract(Name = "ValueListsIndex", Namespace = "http://schemas.aspiremediagroup.net/jobboard/1.0/Beta", ItemName = "ValueList")]
	public partial class ValueListsIndex : DCDictionary<string, ValueList, ValueListsIndex.KVP, ValueListsIndex>
	{
		[DataContract(Name = "ValueListIndexEntry", Namespace = "http://schemas.aspiremediagroup.net/jobboard/1.0/Beta")]
		public new class KVP : DCDictionary<string, ValueList, ValueListsIndex.KVP, ValueListsIndex>.KVP
		{
			[DataMember(Name = "Name")]
			public override string Key
			{
				get;
				set;
			}

			[DataMember(Name = "List")]
			public override ValueList Value
			{
				get;
				set;
			}

			public static implicit operator KeyValuePair<string, ValueList>(KVP source)
			{
				source.ThrowIfNull("source");
				//could piggyback on the base conversion operator, but it looks very strange, believe me.
				return source.AsKeyValuePair();
			}
		}
	}
#endif
}
