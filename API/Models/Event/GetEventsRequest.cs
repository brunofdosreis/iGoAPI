using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/events")]
	public class GetEventsRequest : BaseRequest<Object>, IReturn<GetEventsResponse>
	{
		public override Object GetEntity()
		{
			return null;
		}
	}
}
