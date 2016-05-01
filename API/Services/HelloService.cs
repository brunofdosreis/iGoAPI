using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories.Configuration;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	public class HelloService : BaseService
	{
		public object Any(HelloRequest request)
		{
			new NhibernateManager().CreateDatabase();

			//Hello hello = request.GetEntity();

			//hello.Save();

			//Hello hello2 = new Hello().Get(hello.Id);

			return new HelloResponse(new Hello());
		}

		public object Any(HelloCreateRequest Request)
		{
			new NhibernateManager().CreateDatabase();

			foreach (HelloCreateRequest.Object user in Request.user)
			{
				JsonServiceClient client = new JsonServiceClient ();

				client.AddHeader ("API-Key", "Android");

				client.Post<BaseResponse>("http://127.0.0.1:8080/api/1.5/user", new PostUserRequest()
					{
						facebookToken = user.facebookToken
					}
				);

				client.AddHeader ("Auth-Key", user.facebookToken);

				client.Get<GetEventsResponse>("http://127.0.0.1:8080/api/1.5/events");

				client.Put<BaseResponse>("http://127.0.0.1:8080/api/1.5/user/preferences", new PutUserPreferencesRequest()
					{
						ageEnd = user.ageEnd,
						ageStart = user.ageStart,
						gender = user.gender
					}
				);
			}

			return new BaseResponse();
		}
	}
}
