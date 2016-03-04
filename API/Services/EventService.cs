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
						"events.since(" + unixTimestamp + "){id,name,description,start_time,attending,interested,rsvp_status,rsvp_status=attending,rsvp_status=interested}"
					}
				});

				foreach(dynamic _event in me.events.data)
				{
					if (_event.rsvp_status != "attending" && _event.rsvp_status != "interested")
					{
						continue;
					}

					ulong eventFacebookId = ulong.Parse(_event.id);

					Event Event = new BaseRepository<Event>().List(x => x.FacebookId == eventFacebookId).FirstOrDefault();

					if (Event == null)
					{
						Event = new Event(){
							FacebookId = eventFacebookId,
							User = new List<User>()
						};
					}

					Event.Title = _event.name;
					Event.Description = _event.description;
					Event.Date = DateTime.Parse(_event.start_time);

					List<User> Users = new List<User>(); //Event.User.ToList();

					foreach(dynamic _user in _event.attending.data)
					{
						ulong userFacebookId = ulong.Parse(_user.id);

						User EventUser = new BaseRepository<User>().List(x => x.FacebookId == userFacebookId).FirstOrDefault();

						if (EventUser != null)
						{
							Users.Add(EventUser);
						}
					}

					foreach(dynamic _user in _event.interested.data)
					{
						ulong userFacebookId = ulong.Parse(_user.id);

						User EventUser = new BaseRepository<User>().List(x => x.FacebookId == userFacebookId).FirstOrDefault();

						if (EventUser != null)
						{
							Users.Add(EventUser);
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

			IQueryable<Match> Matches = new BaseRepository<Match> ().List (x => x.Event.Id == Event.Id &&
				(
					(x.FirstUser.Id == User.Id && x.IsFirstUserLike != null) || 
					(x.SecondUser.Id == User.Id && x.IsSecondUserLike != null)
				)
			);

			List<User> Users = Event.User.Where(x => x.Id != User.Id && 
				!Matches.Any(y => y.FirstUser.Id == x.Id) &&
				!Matches.Any(y => y.SecondUser.Id == x.Id)
			).ToList();

			int age = (new DateTime(1, 1, 1) + (DateTime.Now - User.Birthday)).Year - 1;

			Users = Users.Where(x => x.UserPreferences != null &&
				User.UserPreferences.Gender == User.Gender &&
				(
					age >= User.UserPreferences.AgeStart &&
					age <= User.UserPreferences.AgeEnd
				)
			).ToList();

			return new GetEventUsersResponse(Users);
		}
	}
}
