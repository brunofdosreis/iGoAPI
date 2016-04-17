using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories.Configuration;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class PushService : BaseService
	{
		public object Any(PutPushRequest Request)
		{
			return new BaseResponse();
		}

		public object Any(DeletePushRequest Request)
		{
			return new BaseResponse();
		}
	}
}
