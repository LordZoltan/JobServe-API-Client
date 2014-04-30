/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JobServe.API
{
#if __ANDROID__
	/// <summary>
	/// This is a non-trivial base class to be used on Xamarin.Android (and possibly Xamarin.iOS) builds which need the ability
	/// to deserialize a dictionary type from XML via the DataContractSerializer when the root element name of that
	/// XML has been customised on the server with the Name property of the CollectionDataContractAttribute.  Unfortunately, 
	/// at the time of development, the [CollectionDataContractAttribute] isn't interpreted properly by the 
	/// DataContractSerializer for dictionary-based types, and the Name property is 
	/// ignored (see https://bugzilla.xamarin.com/show_bug.cgi?id=11881.  This base class provides a class that is *like* a
	/// dictionary but which doesn't actually implement that interface (thus preventing the erroneous serialization code
	/// from kicking in).  You inherit from it (passing some admittedly bonkers generic parameters) and then you can 
	/// decorate your derived type (and it's key type) with the necessary  DataContract, CollectionDataContract and 
	/// DataMember attributes to make it work with the standard dictionary wire format you get from the server.
	/// 
	/// A further explanation of how to use it is too much to go into in comments. There's a separate ValueListsIndex type
	/// geared around Xamarin.Android, however, which inherits from this and demonstrates how to use it.  Since this is a 
	/// PCL build, however, it is, by default excluded from the build.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <typeparam name="TKVP"></typeparam>
	/// <typeparam name="TDictionary"></typeparam>
	public class DCDictionary<TKey, TValue, TKVP, TDictionary> : ICollection<TKVP>
		where TDictionary : DCDictionary<TKey, TValue, TKVP, TDictionary>, new()
		where TKVP : DCDictionary<TKey, TValue, TKVP, TDictionary>.KVP, new()
	{
		/// <summary>
		/// 
		/// </summary>
		[DataContract(Name = "KVPBase", Namespace = "http://schemas.aspiremediagroup.net/jobboard/1.0/Beta")]
		public abstract class KVP
		{
			public abstract TKey Key { get; set; }
			public abstract TValue Value { get; set; }

			private KeyValuePair<TKey, TValue> KeyValuePair
			{
				get
				{
					return new KeyValuePair<TKey, TValue>(Key, Value);
				}
				set
				{
					Key = value.Key;
					Value = value.Value;
				}
			}

			/// <summary>
			/// implicit conversion operator from the base KVP type to a KeyValuePair
			/// 
			/// Note - for conversion to be possible for your derived key type, you need 
			/// to redefine another conversion operator - as C# only looks at the exact type
			/// - not bases.
			/// </summary>
			/// <param name="source"></param>
			/// <returns></returns>
			public static implicit operator KeyValuePair<TKey, TValue>(KVP source)
			{
				source.ThrowIfNull("source");
				return source.AsKeyValuePair();
			}

			/// <summary>
			/// Implicit conversion operator from KeyValuePair to the base KVP type.
			/// 
			/// Creates an instance of the generic type parameter TKVP.
			/// 
			/// Note - for conversion to be possible for your derived key type, you need
			/// to redefine another conversion operator - as C# only looks at the exact type
			/// - not bases.
			/// </summary>
			/// <param name="source"></param>
			/// <returns></returns>
			public static implicit operator KVP(KeyValuePair<TKey, TValue> source)
			{
				var toReturn = new TKVP();
				toReturn.KeyValuePair = source;
				return toReturn;
			}

			/// <summary>
			/// Keys the value pair.
			/// </summary>
			/// <param name="src">The SRC.</param>
			/// <returns></returns>
			public KeyValuePair<TKey, TValue> AsKeyValuePair()
			{
				return KeyValuePair;
			}

			public KVP() { }

			public KVP(TKey key, TValue value)
			{
				Key = key;
				Value = value;
			}

			public KVP(KeyValuePair<TKey, TValue> source)
			{
				KeyValuePair = source;
			}
		}

		private Dictionary<TKey, TValue> _dictionary;

		private ICollection<KeyValuePair<TKey, TValue>> InnerCollection
		{
			get
			{
				return _dictionary;
			}
		}

		public DCDictionary() { _dictionary = new Dictionary<TKey, TValue>(); }
		public DCDictionary(int capacity) { _dictionary = new Dictionary<TKey, TValue>(capacity); }
		public DCDictionary(IEqualityComparer<TKey> comparer) { _dictionary = new Dictionary<TKey, TValue>(comparer); }
		public DCDictionary(IDictionary<TKey, TValue> dictionary) { _dictionary = new Dictionary<TKey, TValue>(dictionary); }
		public DCDictionary(int capacity, IEqualityComparer<TKey> comparer) { _dictionary = new Dictionary<TKey, TValue>(capacity, comparer); }
		public DCDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) { _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer); }

		private void LoadFrom(Dictionary<TKey, TValue> source)
		{
			_dictionary = new Dictionary<TKey, TValue>(source);
		}

		/// <summary>
		/// Returns the inner dictionary.  It can be modified and the changes will be reflected in this.
		/// instance.
		/// </summary>
		/// <returns></returns>
		public Dictionary<TKey, TValue> AsDictionary()
		{
			return _dictionary;
		}

		public static implicit operator Dictionary<TKey, TValue>(DCDictionary<TKey, TValue, TKVP, TDictionary> source)
		{
			source.ThrowIfNull("source");
			return source.AsDictionary();
		}

	#region ICollection<MyDictionary2KeyValuePair> Members

		public void Add(TKVP item)
		{
			item.ThrowIfNull("item");
			InnerCollection.Add(item.AsKeyValuePair());
		}

		public void Clear()
		{
			InnerCollection.Clear();
		}

		public bool Contains(TKVP item)
		{
			item.ThrowIfNull("item");
			return InnerCollection.Contains(item);
		}

		public void CopyTo(TKVP[] array, int arrayIndex)
		{
			KeyValuePair<TKey, TValue>[] intermediateArray = new KeyValuePair<TKey, TValue>[array.Length];
			InnerCollection.CopyTo(intermediateArray, arrayIndex);
			for (int i = arrayIndex; i < Count; i++)
			{
				array[i] = (TKVP)intermediateArray[i];
			}
		}

		public int Count
		{
			get { return _dictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return InnerCollection.IsReadOnly; }
		}

		public bool Remove(TKVP item)
		{
			return InnerCollection.Remove(item);
		}

		#endregion

	#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		//note - the generic enumerable is public
	#region IEnumerable<TKVP> Members

		public IEnumerator<TKVP> GetEnumerator()
		{
			foreach (var kvp in InnerCollection)
			{
				yield return (TKVP)kvp;
			}
		}

		#endregion

	#region IDictionary<TKey,TValue> Members

		public void Add(TKey key, TValue value)
		{
			_dictionary.Add(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public ICollection<TKey> Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool Remove(TKey key)
		{
			return _dictionary.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get { return _dictionary.Values; }
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				_dictionary[key] = value;
			}
		}

		#endregion
	}
#endif
}
