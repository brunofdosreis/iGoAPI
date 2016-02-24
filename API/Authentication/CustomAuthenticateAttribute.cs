using System;
using System.Linq;

using ServiceStack.Web;
using ServiceStack.Host;
using ServiceStack.Auth;
using ServiceStack.Caching;

namespace ServiceStack.ServiceInterface
{
	/// <summary>
	/// Same as AuthenticateAttribute but adds option to keep session
	/// with cors ajax with sessionid in Authorization header
	/// 1. Authorize the regular way, keep the session id (in cookie or just in js)
	/// 2. Send the session id in each request: xhr.setRequestHeader("Session-Id", = "{sessionid}");
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class CustomAuthenticateAttribute : RequestFilterAttribute
	{
		public string Provider { get; set; }
		public string HtmlRedirect { get; set; }

		public CustomAuthenticateAttribute(ApplyTo applyTo)
			: base(applyTo)
		{
			this.Priority = (int)RequestFilterPriority.Authenticate;
		}

		public CustomAuthenticateAttribute()
			: this(ApplyTo.All) { }

		public CustomAuthenticateAttribute(string provider)
			: this(ApplyTo.All)
		{
			this.Provider = provider;
		}

		public CustomAuthenticateAttribute(ApplyTo applyTo, string provider)
			: this(applyTo)
		{
			this.Provider = provider;
		}
			
		public override void Execute(IRequest req, IResponse res, object requestDto)
		{
			if (AuthenticateService.AuthProviders == null) throw new InvalidOperationException("The AuthService must be initialized by calling "
				+ "AuthService.Init to use an authenticate attribute");

			var matchingOAuthConfigs = AuthenticateService.AuthProviders.Where(x =>
				this.Provider.IsNullOrEmpty()
				|| x.Provider == this.Provider).ToList();

			if (matchingOAuthConfigs.Count == 0)
			{
				res.WriteError(req, requestDto, "No OAuth Configs found matching {0} provider"
					.Fmt(this.Provider ?? "any"));
				res.EndRequest();
				return;
			}

			AuthenticateIfDigestAuth(req, res);
			AuthenticateIfBasicAuth(req, res);
			SetSessionIfSessionIdHeader(req, res);

			using (var cache = req.GetCacheClient())
			{

				var sessionId = req.GetSessionId();
				var session = sessionId != null ? cache.Get<IAuthSession>("urn:iauthsession:" + sessionId) : null; //cache.GetSession(sessionId) : null;

				if (session == null || !matchingOAuthConfigs.Any(x => session.IsAuthorized(x.Provider)))
				{
					var htmlRedirect = HtmlRedirect ?? AuthenticateService.HtmlRedirect;

					if (htmlRedirect != null && req.ResponseContentType.MatchesContentType("text/html"))
					{
						var url = htmlRedirect;
						if (url.SafeSubstring(0, 2) == "~/")
						{
							url = req.GetBaseUrl().CombineWith(url.Substring(2));
						}
						url = url.AddQueryParam("redirect", req.AbsoluteUri);
						res.RedirectToUrl(url);
						return;
					}

					AuthProvider.HandleFailedAuth(matchingOAuthConfigs[0], session, req, res);
				}
			}
		}

		private string sessionIdFromHeader(IRequest httpReq)
		{
			var sessionId = httpReq.Headers["Session-Id"];
			return sessionId;
		}

		private void SetSessionIfSessionIdHeader(IRequest req, IResponse res)
		{
			var tokenSessionId = sessionIdFromHeader(req);
			if (tokenSessionId != null)
			{
				req.Items[SessionFeature.SessionId] = tokenSessionId;
				req.Items[SessionFeature.PermanentSessionId] = tokenSessionId;
			}
		}

		public static void AuthenticateIfBasicAuth(IRequest req, IResponse res)
		{
			//Need to run SessionFeature filter since its not executed before this attribute (Priority -100)
			SessionFeature.AddSessionIdToRequestFilter(req, res, null); //Required to get req.GetSessionId()

			var userPass = req.GetBasicAuthUserAndPassword();
			if (userPass != null)
			{
				AuthenticateService authService = req.TryResolve<AuthenticateService>();

				//authService.Request = new HttpRequestContext(req, res, null);
				//var response = authService.Post(new Authenticate
				authService.Post(new Authenticate
					{
						provider = "credentials",
						UserName = userPass.Value.Key,
						Password = userPass.Value.Value
					});
			}

		}

		public void AuthenticateIfDigestAuth(IRequest req, IResponse res)
		{
			//AuthenticateAttribute.AuthenticateIfDigestAuth(req, res);

			AuthenticateService.AuthProviders
				.Where(x => this.Provider.IsNullOrEmpty() || x.Provider == this.Provider)
				.ToList().OfType<IAuthWithRequest>().Each(x => x.PreAuthenticate(req, res));
		}
	}
}
