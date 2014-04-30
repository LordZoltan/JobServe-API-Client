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

namespace System.Runtime.Serialization
{
	/// <summary>
	/// Stub version of this interface as it's not supported by the framework subset this library is compiled under.
	/// 
	/// Versioning is less of an issue for the JobServe API as new members are always added to types at the end of
	/// existing members, therefore the serializer simply stops deserializing once it's read all the stuff it's
	/// expecting to deserialize.
	/// </summary>
	public interface IExtensibleDataObject
	{
	}
}
