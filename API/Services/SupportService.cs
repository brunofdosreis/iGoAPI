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
			Support Support = request.GetEntity();

			string Platform = Support.Platform;

			List<Support> _Support = new BaseRepository<Support>().List (x => x.Platform == Platform).ToList();

			return new SupportResponse(_Support);
		}
	}
}
