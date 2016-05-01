using System;
using System.Linq;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;

namespace iGO.API.Services
{
	public class BaseService : Service
	{
		public User GetAuthenticatedUser(NHibernate.ISession session)
		{
			string FacebookToken = base.Request.Headers.Get("Auth-Key");

			return new BaseRepository<User>(session)
				.List(x => x.FacebookToken == FacebookToken).FirstOrDefault();
		}

		public BaseService ()
		{
		}
	}
}

