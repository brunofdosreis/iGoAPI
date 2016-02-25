using System;
using System.Net;
using System.Linq;

using ServiceStack.Web;

using iGO.Domain.Entities;
using iGO.Repositories;

namespace ServiceStack.ServiceInterface
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class CustomAuthenticateTokenAttribute : RequestFilterAttribute
	{
		public string Provider { get; set; }

		public CustomAuthenticateTokenAttribute(ApplyTo applyTo)
			: base(applyTo)
		{
			this.Priority = (int)RequestFilterPriority.Authenticate;
		}

		public CustomAuthenticateTokenAttribute()
			: this(ApplyTo.All) { }

		public CustomAuthenticateTokenAttribute(string provider)
			: this(ApplyTo.All)
		{
			this.Provider = provider;
		}

		public CustomAuthenticateTokenAttribute(ApplyTo applyTo, string provider)
			: this(applyTo)
		{
			this.Provider = provider;
		}

		public override void Execute(IRequest req, IResponse res, object requestDto)
		{
			var authKey = req.Headers["Auth-Key"];

			if (authKey.IsNullOrEmpty() ||
				!new BaseRepository<User>().List(x => x.FacebookToken == authKey).Any()) {

				// TODO: Verificar se o token é válido no Facebook e remover verificação abaixo

				if (authKey.IsNullOrEmpty() || (!authKey.Equals("Android") && !authKey.Equals("iOS")))
				{
					throw new HttpError (HttpStatusCode.Unauthorized);
				}
			}
		}
	}
}
