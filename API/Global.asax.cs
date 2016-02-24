using System;
using System.Net;

using ServiceStack;

using Newtonsoft.Json;

using iGO.API.Services;
using iGO.API.Models;
using iGO.API.Authorization;

namespace iGO.API
{
	public class Global : System.Web.HttpApplication
	{
		public class AppHost : AppHostBase
		{
			//Tell ServiceStack the name of your application and where to find your services
			public AppHost() : base("iGO Hello Service", 
				typeof(HelloService).Assembly
			) { }

			public override void Configure(Funq.Container container)
			{
				//register any dependencies your services use, e.g:
				//container.Register<ICacheClient>(new MemoryCacheClient());

				this.PreRequestFilters.Add((req, res) => {
					string apiKey = req.Headers["API-Key"];
					if (apiKey == null || !Clients.VerifyKey(apiKey))
					{
						throw new HttpError(HttpStatusCode.Forbidden);
					}
				});

				//Permit modern browsers (e.g. Firefox) to allow sending of any REST HTTP Method
				Plugins.Add(new CorsFeature());


				/*Plugins.Add(new AuthFeature(() => new AuthUserSession(),
					new IAuthProvider[] {
						new CustomCredentialsAuthProvider(), //HTML Form post of UserName/Password credentials
					}));

				Plugins.Add(new RegistrationFeature());

				container.Register<ICacheClient>(new MemoryCacheClient());
				var userRep = new InMemoryAuthRepository();
				container.Register<IUserAuthRepository>(userRep);*/

				//The IUserAuthRepository is used to store the user credentials etc.
				//Implement this interface to adjust it to your app's data storage

				/*SetConfig(new HostConfig {
					DebugMode = true,
				});*/

				//Handle Exceptions occurring in Services:
				this.ServiceExceptionHandlers.Add((httpReq, request, exception) => 
					{
						//log your exceptions here

						//call default exception handler or prepare your own custom response
						//return DtoUtils.CreateErrorResponse(request, exception);

						throw new HttpError(HttpStatusCode.InternalServerError);
					});

				//Handle Unhandled Exceptions occurring outside of Services
				//E.g. Exceptions during Request binding or in filters:
				this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) => 
					{
						int StatusCode = 500;

						try {
							
							StatusCode = ((HttpError)ex).Status;

						} finally {

							res.StatusCode = StatusCode;

							res.Write(JsonConvert.SerializeObject(
								new BaseResponse() { status = StatusCode, message = ""})
							);

							res.ContentType = MimeTypes.Json;

							res.EndRequest(skipHeaders: true);
						}
					});
			}
		}

		//Initialize your application singleton
		protected void Application_Start(object sender, EventArgs e)
		{
			new AppHost().Init();
		}
	}
}
