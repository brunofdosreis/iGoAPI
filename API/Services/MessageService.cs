using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.API.Helpers;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class MessageService : BaseService
	{
		public object Get(GetMessagesRequest Request)
		{
			Match match = new Match().Get(Request.matchID, ((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			/*
			IEnumerable<Message> message = new BaseRepository<Message>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List (x => 
				x.Match == match 
				&& x.Created >= DateTime.ParseExact(
					Request.date, "yyyy-MM-dd'T'HH:mm:ss'GMT'zzz", CultureInfo.InvariantCulture)
			);
			*/

			IQueryable<Match> Matches = new BaseRepository<Match>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List(x => 
				x.SecondUser != null
				&&
				(
					(x.FirstUser == match.FirstUser && x.SecondUser == match.SecondUser)
					||
					(x.FirstUser == match.SecondUser && x.SecondUser == match.FirstUser)
				)
				&&
				(x.IsFirstUserLike && x.IsSecondUserLike != null && x.IsSecondUserLike == true)
			).OrderBy(y => y.Created);

			Match matchFirst = Matches.FirstOrDefault();

			IEnumerable<Message> message = new BaseRepository<Message>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List (x => 
				x.Match == matchFirst 
				&& x.Created >= DateTime.ParseExact(
					Request.date, "yyyy-MM-dd'T'HH:mm:ss'GMT'zzz", CultureInfo.InvariantCulture)
			);

			if (Request.limit > 0)
			{
				message = message.Take(Request.limit);
			}

			return new GetMessagesResponse(message.ToArray());
		}

		public object Post(PostMessageRequest Request)
		{
			Message message = Request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));
			message.FromUser = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			message.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			message.Match.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			User user = message.ToUser;

			List<DeviceToken> deviceToken = new BaseRepository<DeviceToken>(((NHibernate.ISession)base.Request.Items["hibernateSession"])).List (x =>
				x.User.Id == user.Id).ToList();

			if (deviceToken != null && deviceToken.Any ())
			{
				try
				{
					PushHelper.SendNotification(deviceToken, "Nova mensagem de " + message.FromUser.Name, "message");
				}
				catch {
				}
			}

			return new BaseResponse();
		}
	}
}
