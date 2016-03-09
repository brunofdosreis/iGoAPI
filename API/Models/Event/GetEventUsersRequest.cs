using System;

using ServiceStack;

using iGO.Domain.Entities;
using iGO.Repositories.Extensions;

namespace iGO.API.Models
{
	[Route("/{Version}/event/users")]
	public class GetEventUsersRequest : BaseRequest<Event>, IReturn<GetEventUsersResponse>
	{
		public int ID { get; set; }

		public override Event GetEntity()
		{
			Event Event = new Event().Get(ID);

			return Event;
		}
	}
}
