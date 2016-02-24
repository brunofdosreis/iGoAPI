using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories.Configuration;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class HelloService : BaseService
	{
		public object Any(HelloRequest request)
		{
			NhibernateManager.CreateDatabase();

			Hello hello = request.GetEntity();

			hello.Save();

			/*var Repository = new BaseRepository<Hello>();

			Repository.BeginTransaction();

			Repository.Save(hello);

			Repository.CommitTransaction();

			Hello hello2 = Repository.Get(hello.Id);*/

			Hello hello2 = new Hello().Get(hello.Id);

			return new HelloResponse(hello2);
		}
	} 
}
