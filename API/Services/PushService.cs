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
			DeviceToken deviceToken = Request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			deviceToken.User = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			List<DeviceToken> deviceTokens = new BaseRepository<DeviceToken>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List(x => 
				x.Platform == deviceToken.Platform
					&& x.Token == deviceToken.Token
			).ToList();

			if (deviceTokens == null || !deviceTokens.Any())
			{
				deviceToken.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));
			}

			return new BaseResponse();
		}

		public object Delete(DeletePushRequest Request)
		{
			DeviceToken deviceToken = Request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			deviceToken.User = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			List<DeviceToken> deviceTokens = new BaseRepository<DeviceToken>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List(x => 
				x.Platform == deviceToken.Platform
				&& x.Token == deviceToken.Token
			).ToList();

			if (deviceTokens != null && deviceTokens.Any())
			{
				deviceTokens.First().Delete(((NHibernate.ISession)base.Request.Items["hibernateSession"]));
			}

			return new BaseResponse();
		}
	}
}
