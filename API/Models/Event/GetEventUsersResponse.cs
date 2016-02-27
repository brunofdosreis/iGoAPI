using System;
using System.Collections.Generic;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetEventUsersResponse : BaseResponse
	{
		public new Object data { get; set; }

		public GetEventUsersResponse (IEnumerable<User> Users) : base()
		{
			this.data = new Object(Users);
		}

		public class Object
		{
			public IEnumerable<User> users { get; set; }

			public Object(IEnumerable<iGO.Domain.Entities.User> Users)
			{
				List<User> _users = new List<User>();

				foreach(iGO.Domain.Entities.User _user in Users)
				{
					User u = new User()
					{
						ID = _user.Id,
						name = _user.Name,
						gender = _user.Gender,
						pictures = new List<Picture>()
					};

					foreach(UserPictures _picture in _user.UserPictures)
					{
						u.pictures.Add(new Picture() 
							{
								picture = _picture.Picture
							}
						);
					}

					_users.Add(u);
				}

				users = _users;
			}

			public class User
			{
				public int ID { get; set; }
				public string name { get; set; }
				public string gender { get; set; }
				public List<Picture> pictures { get; set; }
			}

			public class Picture
			{
				public string picture { get; set; }
			}
		}
	}
}
