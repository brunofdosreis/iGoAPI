using System;
using System.Data;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using ServiceStack;
using ServiceStack.ServiceInterface;

using Facebook;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Configuration;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class EventService : BaseService
	{
		public object Get(GetEventsRequest Request)
		{
			User User = base.GetAuthenticatedUser();

			try {

				FacebookClient client = new FacebookClient(User.FacebookToken);

				string unixTimestamp = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

				dynamic me = client.Get("me", new { fields = new[] { 
						"events.since(" + unixTimestamp + "){id,name,description,start_time,attending,rsvp_status=attending}"
					}
				});

				foreach(dynamic _event in me.events.data)
				{
					ulong eventFacebookId = ulong.Parse(_event.id);

					Event Event = new BaseRepository<Event>().List(x => x.FacebookId == eventFacebookId).FirstOrDefault();

					if (Event == null)
					{
						Event = new Event(){
							FacebookId = eventFacebookId,
							Title = _event.name,
							Description = _event.description,
							Date = DateTime.Parse(_event.start_time),
							User = new List<User>()
						};
					}

					List<User> Users = Event.User.ToList();

					foreach(dynamic _user in _event.attending.data)
					{
						ulong userFacebookId = ulong.Parse(_user.id);

						User EventUser = new BaseRepository<User>().List(x => x.FacebookId == userFacebookId).FirstOrDefault();

						if (EventUser != null)
						{
							if (!Users.Any(x => x.FacebookId == userFacebookId))
							{
								Users.Add(EventUser);
							}
						}
					}

					Event.User = Users;

					Event.Save();
				}

			} catch (FacebookOAuthException) {

				throw new HttpError(HttpStatusCode.Unauthorized);
			}

			NhibernateManager.GetSession().Clear();

			User = base.GetAuthenticatedUser();

			return new GetEventsResponse(User.Event);
		}

		public object Get(GetEventUsersRequest Request)
		{
			User User = base.GetAuthenticatedUser();

			Event Event = Request.GetEntity();

			List<Match> Matches = new BaseRepository<Match> ().List (x => x.Event.Id == Event.Id &&
				(
					(x.FirstUser.Id == User.Id && x.IsFirstUserMatch != null) || 
					(x.SecondUser.Id == User.Id && x.IsSecondUserMatch != null)
				)
			).ToList();

			List<User> Users = Event.User.Where(x => x.Id != User.Id && 
				!Matches.Any(y => y.FirstUser.Id == x.Id) &&
				!Matches.Any(y => y.SecondUser.Id == x.Id)
			).ToList();

			return new GetEventUsersResponse(Users);
		}
	}
}
