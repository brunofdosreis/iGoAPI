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

			// TODO: manter eventos antigos
			List<Event> _events = new List<Event>();//User.Event.Where(x => x.Date < DateTime.Now).ToList();

			try {

				FacebookClient client = new FacebookClient(User.FacebookToken);

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

								if (_event.end_time == null)
								{
									Event.EndDate = DateTime.Parse(_event.end_time);
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

			User.Event = _events;

			User.Save();

			NhibernateManager.GetSession().Clear();

			User = base.GetAuthenticatedUser();

			List<Event> Events = User.Event.Where(x => (x.EndDate == null && x.StartDate.AddHours(6) > DateTime.Now) ||
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
			User User = base.GetAuthenticatedUser();

			Event Event = Request.GetEntity();

			/*ulong facebookId = Event.FacebookId;

			List<User> _users = new List<User>(); //Event.User.ToList();

			try
			{
				FacebookClient client = new FacebookClient(User.FacebookToken);

				string after = "";

				do
				{
					dynamic _event = client.Get(facebookId.ToString(), new { fields = new[] { "attending.limit(1000)" + after + "{id}" } } );

					if (_event.attending != null)
					{
						foreach(dynamic _user in _event.attending.data)
						{
							ulong userFacebookId = ulong.Parse(_user.id);

							User EventUser = new BaseRepository<User>().List(x => x.FacebookId == userFacebookId).FirstOrDefault();

							if (EventUser != null)
							{
								_users.Add(EventUser);
							}
						}

						after = ".after(" + _event.attending.paging.cursors.after + ")";
					}
					else
					{
						after = "";
					}

				} while (after != "");

				do
				{
					dynamic _event = client.Get(facebookId.ToString(), new { fields = new[] { "interested.limit(1000)" + after + "{id}" } } );

					if (_event.interested != null)
					{
						foreach(dynamic _user in _event.interested.data)
						{
							ulong userFacebookId = ulong.Parse(_user.id);

							User EventUser = new BaseRepository<User>().List(x => x.FacebookId == userFacebookId).FirstOrDefault();

							if (EventUser != null)
							{
								_users.Add(EventUser);
							}
						}

						after = ".after(" + _event.interested.paging.cursors.after + ")";
					}
					else
					{
						after = "";
					}

				} while (after != "");
			}
			catch (FacebookOAuthException)
			{
				throw new HttpError(HttpStatusCode.Unauthorized);
			}

			Event.User = _users;

			Event.Save();*/

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

			int userAge = (new DateTime(1, 1, 1) + (DateTime.Now - User.Birthday)).Year - 1;

			Users = Users.Where(x => x.UserPreferences != null && User.UserPreferences != null &&
				User.UserPreferences.Gender.Contains(x.Gender) && x.UserPreferences.Gender.Contains(User.Gender)
				&&
				(
					userAge >= x.UserPreferences.AgeStart &&
					userAge <= x.UserPreferences.AgeEnd
				)
				&&
				(
					(new DateTime(1, 1, 1) + (DateTime.Now - x.Birthday)).Year - 1 >= User.UserPreferences.AgeStart &&
					(new DateTime(1, 1, 1) + (DateTime.Now - x.Birthday)).Year - 1 <= User.UserPreferences.AgeEnd
				)
			).ToList();

			return new GetEventUsersResponse(Users.Take(Request.limit));
		}
	}
}
