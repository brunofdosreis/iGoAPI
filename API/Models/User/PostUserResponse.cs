using System;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class PostUserResponse : BaseResponse
	{
		public new Object data { get; set; }

		public PostUserResponse(User User) : base()
		{
			this.data = new Object(User);
		}

		public class Object
		{
			public string email { get; set; }
			public string name { get; set; }

			public Object(User User)
			{
				email = User.Email;
				name = User.Name;
			}
		}
	}
}
