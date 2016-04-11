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

			Event Event = new Event().Get(Request.eventID);

			Match Match = new BaseRepository<Match>().List (x => 
				x.FirstUser == User && x.SecondUser == AuthenticatedUser && 
				x.Event == Event).FirstOrDefault();

			if (Match == null)
			{
				Match = new Match() {
					FirstUser = AuthenticatedUser,
					DateFirstUser = DateTime.Now,
					IsFirstUserLike = Request.isLike,
					SecondUser = User
				};
			}
			else
			{
				Match.DateSecondUser = DateTime.Now;
				Match.IsSecondUserLike = Request.isLike;
			}

			Match.Save();

			return new PostMatchLikeResponse(Match);
		}

		public object Get(GetMatchesRequest Request)
		{
			User User = GetAuthenticatedUser();

			IQueryable<Match> Match = new BaseRepository<Match>().List (x => 
				x.FirstUser == User || x.SecondUser == User);

			GetMatchesResponse Response = new GetMatchesResponse(Match.ToList ());

			Response.data = Response.data.Skip(Request.offset).ToArray();

			if (Request.limit > 0)
			{
				Response.data = Response.data.Take(Request.limit).ToArray();
			}

			return Response;
		}
	}
}
