/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace JobServe.API
{
	/// <summary>
	/// Contains static methods for the types in APITypes.cs
	/// </summary>
	public static class APITypes
	{
		// Note - I only put the T : class constraint on these methods because there 
		// are no Jobs API Types that are value types.

		/// <summary>
		/// Guaranteed to clone an object if its type is one of the API types (Job, JobSearch etc).
		/// 
		/// Cloning is performed by serializing the object to wire format and then deserializing 
		/// it again to a new instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="apiObject">If this is null, then the method returns null also.</param>
		/// <returns></returns>
		public static T CloneAPIObject<T>(T apiObject) where T : class
		{
			if (apiObject == null)
				return null;
			//easily the simplest way to implement this - just write the object to a stream
			//and then read it back again as another 
			DataContractSerializer ser = new DataContractSerializer(typeof(T));
			using (var ms = new MemoryStream())
			{
				ser.WriteObject(ms, apiObject);
				ms.Flush();
				ms.Seek(0, SeekOrigin.Begin);
				return (T)ser.ReadObject(ms);
			}
		}

		/// <summary>
		/// Serializes the passed API object to its XML wire format.
		/// </summary>
		/// <typeparam name="T">Should only ever be one of the API type that's been generated from the Jobs API's schema.</typeparam>
		/// <param name="apiObject">The object to serialize - if null, then the resulting string will be null.</param>
		/// <returns></returns>
		public static string Serialize<T>(T apiObject) where T : class
		{
			if (apiObject == null)
				return null;

			DataContractSerializer ser = new DataContractSerializer(typeof(T));
			using (var ms = new MemoryStream())
			{
				ser.WriteObject(ms, apiObject);
				ms.Flush();
				ms.Seek(0, SeekOrigin.Begin);
				using (var sr = new StreamReader(ms))
				{
					return sr.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Deserializes the specified wire format XML into an API type.
		/// </summary>
		/// <typeparam name="T">Should only ever be one of the API type that's been generated from the Jobs API's schema.</typeparam>
		/// <param name="wireFormatXml">A string containing a Jobs API object serialized into its XML wire format.  If null or empty, then 
		/// the method returns null.</param>
		/// <returns></returns>
		public static T Deserialize<T>(string wireFormatXml) where T : class
		{
			if (string.IsNullOrWhiteSpace(wireFormatXml))
				return default(T);

			DataContractSerializer ser = new DataContractSerializer(typeof(T));
			using (var sr = new StringReader(wireFormatXml))
			{
				using (var xr = XmlReader.Create(sr))
				{
					return (T)ser.ReadObject(xr);
				}
			}
		}

	}
}
