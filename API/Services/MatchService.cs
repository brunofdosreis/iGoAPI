using System;
using System.Collections.Generic;
using System.Linq;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.API.Helpers;
using iGO.Repositories;
using iGO.Repositories.Extensions;
using iGO.Domain.Entities;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class MatchService : BaseService
	{
		public object Post(PostMatchLikeRequest Request)
		{
			User AuthenticatedUser = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			User user = new User().Get(Request.userID, ((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			Event event_ = new Event().Get(Request.eventID, ((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			Match match = new BaseRepository<Match>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List(x => 
				x.FirstUser == user && x.SecondUser == AuthenticatedUser && 
				x.Event == event_).FirstOrDefault();

			if (match == null)
			{
				match = new Match() {
					Event = event_,
					FirstUser = AuthenticatedUser,
					DateFirstUser = DateTime.Now,
					IsFirstUserLike = Request.isLike,
					SecondUser = user
				};
			}
			else
			{
				match.DateSecondUser = DateTime.Now;
				match.IsSecondUserLike = Request.isLike;
			}

			match.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			if (match.IsFirstUserLike
			    && match.IsSecondUserLike != null
			    && match.IsSecondUserLike == true)
			{
				User firstUser = match.FirstUser;

				List<DeviceToken> deviceToken = new BaseRepository<DeviceToken>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List (x =>
					x.User.Id == firstUser.Id).ToList();

				if (deviceToken != null && deviceToken.Any ())
				{
					PushHelper.SendNotification(deviceToken, "Novo Match com " + firstUser.Name, "match");
				}
			}

			return new PostMatchLikeResponse(match);
		}

		public object Get(GetMatchesRequest Request)
		{
			User user = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			IQueryable<Match> Matches = new BaseRepository<Match>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List(x => 
				x.SecondUser != null
				&&
				(x.FirstUser == user || x.SecondUser == user)
				&&
				(x.IsFirstUserLike && x.IsSecondUserLike != null && x.IsSecondUserLike == true)
			).OrderByDescending(y => y.Updated);

			GetMatchesResponse Response = new GetMatchesResponse(user, Matches.ToList());

			Response.data = Response.data.Skip(Request.offset).ToArray();

			if (Request.limit > 0)
			{
				Response.data = Response.data.Take(Request.limit).ToArray();
			}

			return Response;
		}
	}
}
