using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/user")]
	public class PostUserRequest : BaseRequest<User>, IReturn<PostUserResponse>
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
