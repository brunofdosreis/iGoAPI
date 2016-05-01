using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/match/like")]
	public class PostMatchLikeRequest : BaseRequest<Match>, IReturn<PostMatchLikeResponse>
	{
		public int userID { get; set; }
		public int eventID { get; set; }
		public bool isLike { get; set; }

		public override Match GetEntity(NHibernate.ISession session)
		{
			Match Match = new Match();

			return Match;
		}
	}
}
