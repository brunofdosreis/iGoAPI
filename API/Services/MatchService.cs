using System;
using System.Collections.Generic;
using System.Linq;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
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

			User User = new User().Get(Request.userID);

			Event event_ = new Event().Get(Request.eventID);

			Match match = new BaseRepository<Match>().List (x => 
				x.FirstUser == User && x.SecondUser == AuthenticatedUser && 
				x.Event == event_).FirstOrDefault();

			if (match == null)
			{
				match = new Match() {
					Event = event_,
					FirstUser = AuthenticatedUser,
					DateFirstUser = DateTime.Now,
					IsFirstUserLike = Request.isLike,
					SecondUser = User
				};
			}
			else
			{
				match.DateSecondUser = DateTime.Now;
				match.IsSecondUserLike = Request.isLike;
			}

			match.Save();

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
