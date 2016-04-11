using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/matches")]
	public class GetMatchesRequest : BaseRequest<Object>, IReturn<GetMatchesResponse>
	{
		public int limit { get; set; }
		public int offset { get; set; }

		public override Object GetEntity()
		{
			return null;
		}
	}
}
