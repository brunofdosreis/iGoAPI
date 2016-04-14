using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/events", "GET")]
	public class GetEventsRequest : BaseRequest<Object>, IReturn<GetEventsResponse>
	{
		public int limit { get; set; }
		public int offset { get; set; }

		public override Object GetEntity()
		{
			return null;
		}
	}
}
