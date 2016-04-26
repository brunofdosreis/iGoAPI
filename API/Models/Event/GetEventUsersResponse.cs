using System;
using System.Linq;
using System.Collections.Generic;

using iGO.Domain.Entities;
using iGO.Repositories;

namespace iGO.API.Models
{
	public class GetEventUsersResponse : BaseResponse
	{
		public new Object data { get; set; }

		public GetEventUsersResponse (User user, IEnumerable<User> Users) : base()
		{
			this.data = new Object(user, Users);
		}

		public class Object
		{
			public IEnumerable<User> users { get; set; }

			public Object(iGO.Domain.Entities.User user, IEnumerable<iGO.Domain.Entities.User> Users)
			{
				List<User> _users = new List<User>();

				foreach(iGO.Domain.Entities.User _user in Users)
				{
					User u = new User()
					{
						ID = _user.Id,
						name = _user.Name,
						birthday = _user.Birthday.ToString("yyyy-MM-dd'T'HH:mm:ss'GMT'zzz"),
						gender = _user.Gender,
						pictures = new List<Picture>()
					};

					Match match = new BaseRepository<Match>().List(x => x.FirstUser.Id == _user.Id
						|| x.SecondUser.Id ==_user.Id).FirstOrDefault();

					if (match != null)
					{
						u.matchID = match.Id;
					}

					u.events = new BaseRepository<Event>().List(x => x.User.Any(y => y.Id == _user.Id)).Count();

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
				public int? matchID { get; set; }
				public int events { get; set; }
				public string name { get; set; }
				public string birthday { get; set; }
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
