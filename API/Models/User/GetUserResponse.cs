using System;
using System.Linq;
using System.Collections.Generic;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Extensions;

namespace iGO.API.Models
{
	public class GetUserResponse : BaseResponse
	{
		public new Object data { get; set; }

		public GetUserResponse(User User, NHibernate.ISession session) : base()
		{
			this.data = new Object(User, session);
		}

		public class Object
		{
			public string email { get; set; }
			public string name { get; set; }
			public string gender { get; set; }
			public string birthday { get; set; }
			public IEnumerable<Picture> pictures { get; set; }
			//public IEnumerable<Event> events { get; set; }
			public int events { get; set; }

			public Object(User User, NHibernate.ISession session)
			{
				email = User.Email;
				name = User.Name;
				gender = User.Gender;
				birthday = User.Birthday.ToString ("yyyy-MM-dd'T'HH:mm:ss'GMT'zzz");

				List<Picture> _pictures = new List<Picture>();

				foreach(UserPictures picture in User.UserPictures)
				{
					_pictures.Add(new Picture()
						{
							ID = picture.Id,
							picture = picture.Picture,
							isDefault = picture.IsDefault
						}
					);
				}

				pictures = _pictures;

				/*
				List<Event> _events = new List<Event>();

				if (User.Event != null)
				{
					foreach(iGO.Domain.Entities.Event _event in User.Event)
					{
						_events.Add(new Event()
							{
								ID = _event.Id,
								title = _event.Title,
								desciption = _event.Description,
								date = _event.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss'GMT'zzz")
							}
						);
					}
				}
				*/

				events = new BaseRepository<Event>(session).List(x => x.User.Any(y => y.Id == User.Id)).Count();
			}

			public class Picture
			{
				public int ID { get; set; }
				public string picture { get; set; }
				public bool isDefault { get; set; }
			}

			/*
			public class Event
			{
				public int ID { get; set; }
				public string title { get; set; }
				public string desciption { get; set; }
				public string date { get; set; }
			}
			*/
		}
	}
}
