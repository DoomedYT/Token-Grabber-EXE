using System;
using System.Net;

namespace Token_Grabber
{
	public class WebClientEx : WebClient
	{
		private CookieContainer container = new CookieContainer();

		public CookieContainer CookieContainer
		{
			get
			{
				return container;
			}
			set
			{
				container = value;
			}
		}

		public WebClientEx(CookieContainer container)
		{
			this.container = container;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = base.GetWebRequest(address);
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.AllowAutoRedirect = false;
			if (httpWebRequest != null)
			{
				httpWebRequest.CookieContainer = container;
			}
			return webRequest;
		}

		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = base.GetWebResponse(request, result);
			ReadCookies(webResponse);
			return webResponse;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse webResponse = base.GetWebResponse(request);
			ReadCookies(webResponse);
			return webResponse;
		}

		private void ReadCookies(WebResponse r)
		{
			HttpWebResponse httpWebResponse = r as HttpWebResponse;
			if (httpWebResponse != null)
			{
				CookieCollection cookies = httpWebResponse.Cookies;
				container.Add(cookies);
			}
		}
	}
}
