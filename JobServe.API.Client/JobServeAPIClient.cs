using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JobServe.API
{
	/// <summary>
	/// A basic client for the JobServe API.
	/// </summary>
	public class JobServeAPIClient
	{
		private const string DefaultToken = "6t6eXsLhq8JuESV3ZjXUQCdUzxzcpM3r3kZif239d-85w2Abc-avgvmelgAouveRxGouN6Yx-fAmVfT54d-MYsw5ohP_tqC-uIrQviJY8X8vRx_0Do4AfDdqaYVJCRYqiv2MQVTlB9aEX6z7r748QMYRWynYAZF8Y6Xd3hEYzQU";
		/// <summary>
		/// constant for the hostname to use on all API requests
		/// </summary>
		private const string APIHostName = "services.jobserve.com/";
		/// <summary>
		/// constant for the user agent to be sent on all API requests
		/// </summary>
		private const string UserAgent = "JobServe-API-Client-Open";

		private readonly string APIToken;

		/// <summary>
		/// Constructs a new instance of the client.
		/// </summary>
		/// <param name="apiToken">Required. The API token you've been issued with after requesting access
		/// at https://services.jobserve.com/Developers/Register.</param>
		public JobServeAPIClient(string apiToken = DefaultToken)
		{
			if (apiToken == null)
				throw new ArgumentNullException("apiToken");
			else if (apiToken.Trim().Length == 0)
				throw new ArgumentException("String cannot be entirely whitespace, or empty", "apiToken");

			APIToken = apiToken;
		}

		#region private helpers
		
		private Uri MakeRequestURI(bool secure, string relativePathAndQuery)
		{
			int queryPos = relativePathAndQuery.IndexOf("?");
			string query = string.Empty;
			if(queryPos != -1)
			{
				query = relativePathAndQuery.Substring(queryPos + 1);
				//strip the query string off the relativePathAndQuery,
				//just reusing the local variable introduced by the parameter here.
				relativePathAndQuery = relativePathAndQuery.Substring(0, queryPos -1);
			}

			return new UriBuilder(secure ? "https" : "http", APIHostName)
			{
				Path = relativePathAndQuery,
				Query = query
			}.Uri;
		}

		private void AddStandardHeaders(HttpRequestMessage request)
		{
			//set the request headers
			if (!string.IsNullOrWhiteSpace(APIToken))
				request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", APIToken);
			//set the accept header
			request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
			//set the language header from the current UI culture
			request.Headers.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(CultureInfo.CurrentUICulture.Name));
		}

		private HttpClient CreateClient(bool enableCompression)
		{
			return new HttpClient(new HttpClientHandler() {
				AutomaticDecompression = enableCompression ? DecompressionMethods.GZip : DecompressionMethods.None 
			});
		}

		private async Task<TResponse> Deserialize<TResponse>(HttpResponseMessage response)
		{
			using (var respStream = await response.Content.ReadAsStreamAsync())
			{
				//create a DataContractSerializer to read the type
				var dcs = new DataContractSerializer(typeof(TResponse));
				return (TResponse)dcs.ReadObject(respStream);
			}
		}

		#endregion

		/// <summary>
		/// Returns a task that executes a get request to the specified API path, deserializing the 
		/// result as the given type <typeparamref name="TResponse"/>.
		/// 
		/// In it's current form, this method is not suitable for handling requests that return empty
		/// responses.  However, on the public API no such methods exist.
		/// </summary>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="relativePathAndQuery"></param>
		/// <param name="enableCompression"></param>
		/// <param name="secure"></param>
		/// <returns></returns>
		public async Task<TResponse> Get<TResponse>(string relativePathAndQuery, bool enableCompression = true, bool secure = false)
		{
			//TODO: add parameter validation
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, MakeRequestURI(secure, relativePathAndQuery));
			AddStandardHeaders(request);

			using (var client = CreateClient(enableCompression))
			{
				var response = await client.SendAsync(request);
				response.EnsureSuccessStatusCode();
				return await Deserialize<TResponse>(response);
			}
		}

		/// <summary>
		/// Returns a task that executes a POST/PUT etc request with an entity body, deserializing
		/// the result as the given type <typeparamref name="TResponse"/>.
		/// 
		/// As with <see cref="Get"/>, this method is not suitable for handling requests that
		/// return empty responses.
		/// </summary>
		/// <typeparam name="TContent"></typeparam>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="relativePathAndQuery"></param>
		/// <param name="method"></param>
		/// <param name="content"></param>
		/// <param name="enableCompression"></param>
		/// <param name="secure"></param>
		/// <returns></returns>
		public async Task<TResponse> Send<TContent, TResponse>(string relativePathAndQuery, HttpMethod method, TContent content, bool enableCompression = true, bool secure = false)
		{
			//TODO: add parameter validation
			if (method.Equals(HttpMethod.Get) || method.Equals(HttpMethod.Head) || method.Equals(HttpMethod.Options))
				throw new ArgumentException(string.Format("HttpMethod.{0} is not allowed for this operation", method.ToString()));

			HttpRequestMessage request = new HttpRequestMessage(method, MakeRequestURI(secure, relativePathAndQuery));
			AddStandardHeaders(request);

			using (var client = CreateClient(enableCompression))
			{
				HttpResponseMessage response = null;
				//we serialize the request object to a memory stream, which is then 
				//set on the request as its content
				using (var ms = new MemoryStream())
				{
					var dcs = new DataContractSerializer(typeof(TContent));
					dcs.WriteObject(ms, content);
					//this shouldn't be necessary, but anyway
					await (ms.FlushAsync());
					//now seek back to the start of the stream, 
					//otherwise no content will be sent.
					ms.Seek(0, SeekOrigin.Begin);
					//construct the content to be set on the request message,
					//and set it.
					var streamContent = new StreamContent(ms);
					streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
					request.Content = streamContent;
					response = await client.SendAsync(request);
				}

				return await Deserialize<TResponse>(response);
			}
		}
	}
}
