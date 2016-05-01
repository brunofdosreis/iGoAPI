using System;

using ServiceStack;

using iGO.Domain.Entities;
using iGO.Repositories.Extensions;

namespace iGO.API.Models
{
	[Route("/{Version}/user", "GET")]
	[Route("/{Version}/user/{ID}", "GET")]
	public class GetUserRequest : BaseRequest<User>, IReturn<GetUserResponse>
	{
		public int ID { get; set; }

		public override User GetEntity(NHibernate.ISession session)
		{
			User User = new User ().Get(ID, session);

			return User;
		}
	}
}
