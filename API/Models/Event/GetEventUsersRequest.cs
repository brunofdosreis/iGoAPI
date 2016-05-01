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
		public int limit { get; set; }

		public override Event GetEntity(NHibernate.ISession session)
		{
			Event Event = new Event().Get(ID, session);

			return Event;
		}
	}
}
