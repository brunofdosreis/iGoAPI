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
			User AuthenticatedUser = GetAuthenticatedUser();

			User user = new User().Get(Request.userID);

			Event event_ = new Event().Get(Request.eventID);

			Match match = new BaseRepository<Match>().List (x => 
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

			match.Save();

			if (match.IsFirstUserLike
			    && match.IsSecondUserLike != null
			    && match.IsSecondUserLike == true)
			{
				User firstUser = match.FirstUser;

				List<DeviceToken> deviceToken = new BaseRepository<DeviceToken> ().List (x =>
					x.User.Id == firstUser.Id).ToList();

				if (deviceToken != null && deviceToken.Any ())
				{
					PushHelper.SendNotification(deviceToken, "Novo Match!");
				}
			}

			return new PostMatchLikeResponse(match);
		}

		public object Get(GetMatchesRequest Request)
		{
			User user = GetAuthenticatedUser();

			IQueryable<Match> Matches = new BaseRepository<Match>().List(x => 
				x.SecondUser != null
				&&
				(x.FirstUser == user || x.SecondUser == user)
				&&
				(x.IsFirstUserLike && x.IsSecondUserLike != null && x.IsSecondUserLike == true)
			);

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
