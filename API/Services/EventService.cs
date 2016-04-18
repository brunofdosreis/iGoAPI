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
			User user = base.GetAuthenticatedUser();

			List<Event> _events = new List<Event>();

			if (user.Event != null) {

				_events = user.Event.Where (x => 
					(x.EndDate == null && x.StartDate < DateTime.Now.AddHours (6))
					|| x.EndDate < DateTime.Now
				).ToList ();
			}

			try {

				FacebookClient client = new FacebookClient(user.FacebookToken);

				string unixTimestamp = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

				string after = "";

				if (Request.offset == 0)
				{
					do
					{
						dynamic me = client.Get("me", new { fields = new[] {
								"events.since(" + unixTimestamp + ").limit(200)" + after + "{id,name,description,start_time,end_time,rsvp_status}"
							}
						});

						if (me.events != null)
						{
							foreach(dynamic _event in me.events.data)
							{
								if (_event.rsvp_status != "attending" && _event.rsvp_status != "unsure")
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
								Event.StartDate = DateTime.Parse(_event.start_time);

								if (_event.end_time != null)
								{
									Event.EndDate = DateTime.Parse(_event.end_time);

								} else {

									Event.EndDate = null;
								}

								_events.Add(Event);
							}

							after = ".after(" + me.events.paging.cursors.after + ")";
						}
						else
						{
							after = "";
						}

					} while (after != "");
				}
			}
			catch (FacebookOAuthException)
			{
				throw new HttpError(HttpStatusCode.Unauthorized);
			}

			user.Event = _events;

			user.Save();

			NhibernateManager.GetSession().Clear();

			user = base.GetAuthenticatedUser();

			List<Event> Events = user.Event.Where(x => (x.EndDate == null && x.StartDate.AddHours(6) > DateTime.Now) ||
				x.EndDate > DateTime.Now).ToList();

			Events = Events.Skip(Request.offset).ToList();

			if (Request.limit > 0)
			{
				Events = Events.Take(Request.limit).ToList();
			}

			return new GetEventsResponse(Events);
		}

		public object Get(GetEventUsersRequest Request)
		{
			User user = base.GetAuthenticatedUser();

			Event Event = Request.GetEntity();

			IQueryable<Match> Matches = new BaseRepository<Match>().List(x => x.Event.Id == Event.Id &&
				(
					(x.FirstUser.Id == user.Id && x.IsFirstUserLike != null) || 
					(x.SecondUser.Id == user.Id && x.IsSecondUserLike != null)
				)
			);

			IEnumerable<User> Users = Event.User.Where(x => x.Id != user.Id
				&& !Matches.Any(y => y.FirstUser.Id == x.Id || y.SecondUser.Id == x.Id)
			);

			int userAge = (new DateTime(1, 1, 1) + (DateTime.Now - user.Birthday)).Year - 1;

			Users = Users.Where (x => x.UserPreferences != null && user.UserPreferences != null
				&& user.UserPreferences.Gender.Contains (x.Gender) && x.UserPreferences.Gender.Contains (user.Gender)
			);

			Users = Users.Where (x => x.UserPreferences != null && user.UserPreferences != null
				&&
				(
				    userAge >= x.UserPreferences.AgeStart &&
				    userAge <= x.UserPreferences.AgeEnd
				)
			);

			Users = Users.Where (x => x.UserPreferences != null && user.UserPreferences != null
				&&
				(
					(new DateTime(1, 1, 1) + (DateTime.Now - x.Birthday)).Year - 1 >= user.UserPreferences.AgeStart &&
					(new DateTime(1, 1, 1) + (DateTime.Now - x.Birthday)).Year - 1 <= user.UserPreferences.AgeEnd
				)
			);

			if (Request.limit > 0)
			{
				Users = Users.Take(Request.limit);
			}

			return new GetEventUsersResponse(user, Users);
		}
	}
}
