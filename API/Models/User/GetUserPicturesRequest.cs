using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/user/pictures", "GET")]
	public class GetUserPicturesRequest : BaseRequest<Object>, IReturn<GetUserPicturesResponse>
	{
		public override Object GetEntity(NHibernate.ISession session)
		{
			return null;
		}
	}
}
