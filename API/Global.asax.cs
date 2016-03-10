using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

using ServiceStack;

using Newtonsoft.Json;

using iGO.Domain.Entities;
using iGO.Repositories;
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
			public AppHost() : base("iGO-API", 
				typeof(BaseService).Assembly
			) { }

			public override RouteAttribute[] GetRouteAttributes(Type requestType)
			{
				var routes = base.GetRouteAttributes(requestType);
				routes.Each(x => x.Path = "/api" + x.Path);
				return routes;
			}

			public override void Configure(Funq.Container container)
			{
				//register any dependencies your services use, e.g:
				//container.Register<ICacheClient>(new MemoryCacheClient());

				this.PreRequestFilters.Add((httpReq, httpRes) => {
					httpReq.UseBufferedStream = true;
				});

				this.PreRequestFilters.Add((req, res) => {
					string apiKey = req.Headers["API-Key"];
					if (apiKey == null || !Clients.VerifyKey(apiKey))
					{
						throw new HttpError(HttpStatusCode.Forbidden);
					}
				});

				this.GlobalRequestFilters.Add((req, res, obj) => {

					string Version = "";

					try
					{
						Version = ((BaseRequest)obj).Version;
					}
					finally
					{
						if (Version != null)
						{
							List<Support> Support = new BaseRepository<Support>().List (x => x.Platform == "API").ToList();

							Support oldest = Support.FirstOrDefault(x => x.Type == "oldest");

							if (oldest != null)
							{
								if (String.Compare(oldest.Version, Version) > 0)
								{
									throw new HttpError(HttpStatusCode.Gone);
								}
							}

							Support latest = Support.FirstOrDefault(x => x.Type == "latest");

							if (latest != null)
							{
								if (String.Compare(latest.Version, Version) < 0)
								{
									throw new HttpError(HttpStatusCode.NotImplemented);
								}
							}
						}
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

				SetConfig(new HostConfig { 
					EnableFeatures = Feature.All.Remove(Feature.Metadata)
				});

				//Handle Exceptions occurring in Services:
				this.ServiceExceptionHandlers.Add((httpReq, request, exception) => 
					{
						//log your exceptions here

						//call default exception handler or prepare your own custom response
						//return DtoUtils.CreateErrorResponse(request, exception);

						if (exception.GetType() == typeof(ServiceStack.HttpError))
						{
							throw exception;
						}

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
