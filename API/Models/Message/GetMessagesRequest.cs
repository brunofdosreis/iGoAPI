using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/messages")]
	public class GetMessagesRequest : BaseRequest<Object>, IReturn<PostMatchLikeResponse>
	{
		public int matchID { get; set; }
		public string date { get; set; }
		public int limit { get; set; }

		public override Object GetEntity()
		{
			return null;
		}
	}
}
