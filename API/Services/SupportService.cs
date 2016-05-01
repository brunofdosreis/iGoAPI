using System.Linq;
using System.Collections.Generic;

using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Configuration;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	public class SupportService : BaseService
	{
		public object Any(SupportRequest request)
		{
			Support Support = request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			string Platform = Support.Platform;

			List<Support> _Support = new BaseRepository<Support>(((NHibernate.ISession)base.Request.Items["hibernateSession"]))
				.List (x => x.Platform == Platform).ToList();

			return new SupportResponse(_Support);
		}
	}
}
