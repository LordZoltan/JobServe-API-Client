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
using System.Text;
using System.Threading.Tasks;

namespace JobServe.API
{
	/// <summary>
	/// Used to download collection data from the web API and turn it into a service that
	/// access the underlying collection as a series, but also perform key-based lookups 
	/// in a dictionary-like fashion.
	/// 
	/// Specific services simply inherit from this, wrap the inner constructor, and supply 
	/// implementations for the abstract methods (in addition to adding their interface implementation).
	/// </summary>
	/// <typeparam name="T">Type of each individual item in the collection</typeparam>
	/// <typeparam name="TCollection">Type of the collection of T, must be derived from generic List of T</typeparam>
	public abstract class RemoteCollectionService<TKey, T, TCollection> : IRemoteCollectionService<TCollection> where TCollection : List<T>
	{
		protected class Index
		{
			public readonly T[] AllItemsArray;
			public readonly TCollection AllItems;
			public readonly Dictionary<TKey, T> Lookup;

			public Index(TCollection allItems, Dictionary<TKey, T> lookup)
			{
				AllItems = allItems;
				AllItemsArray = AllItems.ToArray();
				Lookup = lookup;
			}
		}
		private readonly IClient _client;
		private readonly IWebServiceObjectCache Cache;

		private Lazy<Task<Index>> _index;
		private Lazy<Task<TCollection>> _collection;

		public IClient Client { get { return _client; } }

		/// <summary>
		/// Creates the base collection service.
		/// </summary>
		/// <param name="client">Required, a client through which web requests are to be made.</param>
		/// <param name="cache">Optional, an object cache to be used in avoiding repeated web requests to the same
		/// resource if the remote version doesn't change.</param>
		public RemoteCollectionService(IClient client, IWebServiceObjectCache cache = null)
		{
			client.ThrowIfNull("client");
			_client = client;
			Cache = cache;
			//cache the task for the collection so we only load it once.
			_collection = new Lazy<Task<TCollection>>(() => Cache != null ? Cache.GetObject(c => LoadAll(c)) : LoadAll(Client));
			_index = new Lazy<Task<Index>>(() => LoadIndex());
		}

		private async Task<Index> LoadIndex()
		{
			TCollection allItems = await _collection.Value;
			return new Index(allItems, (allItems ?? Enumerable.Empty<T>()).Where(i => i != null).ToDictionary(i => GetKeyFor(i), GetKeyComparer()));
		}

		///// <summary>
		///// this one is used when an object cache was passed to this instance on construction.
		///// </summary>
		///// <param name="cache"></param>
		///// <returns></returns>
		//protected Task<TCollection> LoadAll(IWebServiceObjectCache cache)
		//{
		//	return cache.GetObject(c => LoadAll(c));
		//}

		/// <summary>
		/// Gets the key comparer to be used to compare indexes for the underlying collection.
		/// 
		/// The base implementation returns the default key comparer for the type.
		/// </summary>
		/// <returns>A key comparer.</returns>
		protected virtual IEqualityComparer<TKey> GetKeyComparer() { return EqualityComparer<TKey>.Default; }

		/// <summary>
		/// Your implementation will retrieve the underlying collection from the client.
		/// </summary>
		/// <param name="client">The client through which data is to be loaded.</param>
		/// <returns></returns>
		protected abstract Task<TCollection> LoadAll(IClient client);

		/// <summary>
		/// Used to derive a key for an object that is loaded from the collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected abstract TKey GetKeyFor(T item);

		/// <summary>
		/// Gets one or more items from the index by keyed lookup.
		/// 
		/// Keys that are not present in the dictionary built from the remote data
		/// will yield a null in the same position in the returned array.
		/// </summary>
		/// <param name="ids">The ids.</param>
		/// <returns></returns>
		protected async Task<T[]> GetItems(IEnumerable<TKey> ids = null)
		{
			var index = await _index.Value;
			if (ids == null)
				return index.AllItemsArray;
			else
			{
				return ids.Select(id =>
				{
					T toReturn = default(T);
					index.Lookup.TryGetValue(id, out toReturn);
					return toReturn;
				}).ToArray();
			}
		}

		/// <summary>
		/// Gets all keys in the dictionary.
		/// </summary>
		/// <returns></returns>
		protected async Task<IEnumerable<TKey>> GetAllKeys()
		{
			var index = await _index.Value;
			return index.Lookup.Keys;
		}

		/// <summary>
		/// Gets an item by it's key that was previously assigned through a call to <see cref="GetKeyFor"/>
		/// </summary>
		/// <param name="id">The ID.</param>
		/// <returns></returns>
		protected async Task<T> GetItem(TKey id)
		{
			id.ThrowIfNull("id");

			T toReturn = default(T);
			(await _index.Value).Lookup.TryGetValue(id, out toReturn);
			return toReturn;
		}

		/// <summary>
		/// Returns the underlying collection retrieved from the services.  Useful for binding list views, for example.
		/// </summary>
		/// <returns></returns>
		public Task<TCollection> GetUnderlyingCollection()
		{
			return _collection.Value;
		}
	}

	/// <summary>
	/// Specialised RemoteCollectionService where the key is expected to be a string - this is the most common
	/// case across the Web API.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TCollection"></typeparam>
	public abstract class RemoteCollectionService<T, TCollection> : RemoteCollectionService<string, T, TCollection>
		where TCollection : List<T>
	{
		public RemoteCollectionService(IClient client, IWebServiceObjectCache cache = null) : base(client, cache) { }

		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return StringComparer.OrdinalIgnoreCase;
		}
	}
}
