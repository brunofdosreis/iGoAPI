using System;
using System.Linq;
using System.Collections.Generic;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Configuration;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class PushService : BaseService
	{
		public object Put(PutPushRequest Request)
		{
			DeviceToken deviceToken = Request.GetEntity();

			deviceToken.User = GetAuthenticatedUser();

			List<DeviceToken> deviceTokens = new BaseRepository<DeviceToken>().List(x => 
				x.Platform == deviceToken.Platform
					&& x.Token == deviceToken.Token
			).ToList();

			if (deviceToken == null || !deviceTokens.Any())
			{
				deviceToken.Save();
			}

			return new BaseResponse();
		}

		public object Any(DeletePushRequest Request)
		{
			DeviceToken deviceToken = Request.GetEntity();

			deviceToken.User = GetAuthenticatedUser();

			List<DeviceToken> deviceTokens = new BaseRepository<DeviceToken>().List(x => 
				x.Platform == deviceToken.Platform
				&& x.Token == deviceToken.Token
			).ToList();

			if (deviceToken == null || !deviceTokens.Any())
			{
				deviceToken.Delete();
			}

			return new BaseResponse();
		}
	}
}
