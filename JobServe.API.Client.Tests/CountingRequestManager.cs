using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobServe.API.Tests
{
	/// <summary>
	/// Diagnostic version of the IWebRequestManager interface that counts the number of requests
	/// and responses made.
	/// </summary>
	internal class CountingRequestManager : WebRequestManager
	{
		private int _requestsSent;

		public int RequestsSent
		{
			get { 
				return _requestsSent; 
			}
		}

		private int _responsesReceived;

		public int ResponsesReceived
		{
			get { return _responsesReceived; }
		}

		public CountingRequestManager(string token = WebRequestManager.DefaultToken)
			: base(token ?? WebRequestManager.DefaultToken)
		{

		}

		public override Task<TResponse> Get<TResponse>(string relativePathAndQuery, bool enableCompression = true, bool secure = false)
		{
			Interlocked.Increment(ref _requestsSent);
			return base.Get<TResponse>(relativePathAndQuery, enableCompression, secure).ContinueWith((t) => {
				Interlocked.Increment(ref _responsesReceived);
				return t.Result;
			});
		}

		public override Task<TResponse> Send<TContent, TResponse>(string relativePathAndQuery, System.Net.Http.HttpMethod method, TContent content, bool enableCompression = true, bool secure = false)
		{
			Interlocked.Increment(ref _requestsSent);

			return base.Send<TContent, TResponse>(relativePathAndQuery, method, content, enableCompression, secure).ContinueWith((t) => {
				Interlocked.Increment(ref _responsesReceived);
				return t.Result;
			});
		}
	}
}
