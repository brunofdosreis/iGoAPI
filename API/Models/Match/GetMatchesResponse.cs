using System;
using System.Linq;
using System.Collections.Generic;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetMatchesResponse : BaseResponse
	{
		public new Object[] data { get; set; }

		public GetMatchesResponse(User user, List<Match> matches)
		{
			data = new Object[] {};

			List<Object> Matches = new List<Object>();

			foreach (Match match in matches)
			{
				bool found = false;

				foreach (Object m in Matches) {

					if (m.user.id == match.FirstUser.Id || 
						m.user.id == match.SecondUser.Id) {

						List<string> events = new List<string>(m.events);

						events.Add (match.Event.Title);

						m.events = events.ToArray();

						found = true;
					}
				}

				if (!found) {

					Object.User u = new Object.User();

					if (match.FirstUser.Id == user.Id)
					{
						u = new Object.User() {
							id = match.SecondUser.Id,
							name = match.SecondUser.Name,
							birthday = match.SecondUser.Birthday.ToString ("yyyy-MM-dd'T'HH:mm:ss'GTM'zzz"),
							gender = match.SecondUser.Gender,
							picture = match.SecondUser.UserPictures.First (x => x.IsDefault).Picture
						};
					}
					else
					{
						u = new Object.User() {
							id = match.FirstUser.Id,
							name = match.FirstUser.Name,
							birthday = match.FirstUser.Birthday.ToString ("yyyy-MM-dd'T'HH:mm:ss'GTM'zzz"),
							gender = match.FirstUser.Gender,
							picture = match.FirstUser.UserPictures.First (x => x.IsDefault).Picture
						};
					}

					Matches.Add (new Object
						{
							ID = match.Id,
							events = new string[]{ match.Event.Title },
							user = u
						}
					);
				}

				data = Matches.ToArray();
			}
		}

		public class Object
		{
			public int ID { get; set; }
			public string[] events { get; set; }
			public User user { get; set; }

			public class User
			{
				public int id { get; set; }
				public string name { get; set; }
				public string birthday { get; set; }
				public string gender { get; set; }
				public string picture { get; set; }
			}
		}
	}
}
