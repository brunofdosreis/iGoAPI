using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/user/preferences", "GET")]
	public class GetUserPreferencesRequest : BaseRequest<Object>, IReturn<GetUserPreferencesResponse>
	{
		public override Object GetEntity(NHibernate.ISession session)
		{
			return null;
		}
	}
}
