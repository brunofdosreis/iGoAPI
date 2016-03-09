using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/user")]
	public class PostUserRequest : BaseRequest<User>, IReturn<BaseResponse>
	{
		public string facebookToken { get; set; }

		public override User GetEntity()
		{
			User User = new User();

			User.FacebookToken = facebookToken;

			return User;
		}
	}
}
