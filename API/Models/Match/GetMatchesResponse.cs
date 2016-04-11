﻿using System;
using System.Linq;
using System.Collections.Generic;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetMatchesResponse : BaseResponse
	{
		public new Object[] data { get; set; }

		public GetMatchesResponse(List<Match> Match)
		{
			List<Object> Matches = new List<Object>();

			foreach (Match match in Match)
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

					Matches.Add (new Object
						{
							ID = match.Id,
							events = new string[]{ match.Event.Title },
							user = new Object.User() {
								id = match.FirstUser.Id,
								name = match.FirstUser.Name,
								gender = match.FirstUser.Gender,
								picture = match.FirstUser.UserPictures.First(x => x.IsDefault).Picture
							}
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
				public string gender { get; set; }
				public string picture { get; set; }
			}
		}
	}
}
