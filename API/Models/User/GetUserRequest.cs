using System;

using ServiceStack;

using iGO.Domain.Entities;
using iGO.Repositories.Extensions;

namespace iGO.API.Models
{
	[Route("/{Version}/user/{ID}")]
	public class GetUserRequest : BaseRequest<User>, IReturn<GetUserResponse>
	{
		public int ID { get; set; }

		public override User GetEntity()
		{
			User User = new User().Get(ID);

			return User;
		}
	}
}
