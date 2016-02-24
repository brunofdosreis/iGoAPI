using System;
using System.Collections.Generic;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetUserResponse : BaseResponse
	{
		public new Object data;

		public GetUserResponse(User User) : base()
		{
			this.data = new Object(User);
		}

		public class Object
		{
			public string email { get; set; }
			public string name { get; set; }
			public IEnumerable<Picture> pictures { get; set; }
			public IEnumerable<Event> events { get; set; }

			public Object(User User)
			{
				email = User.Email;
				name = User.Name;

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

				List<Event> _events = new List<Event>();

				foreach(iGO.Domain.Entities.Event _event in User.Event)
				{
					_events.Add(new Event()
						{
							ID = _event.Id,
							title = _event.Title,
							desciption = _event.Description
						}
					);
				}

				events = _events;
			}

			public class Picture
			{
				public int ID { get; set; }
				public string picture { get; set; }
				public bool isDefault { get; set; }
			}

			public class Event
			{
				public int ID { get; set; }
				public string title { get; set; }
				public string desciption { get; set; }
			}
		}
	}
}
