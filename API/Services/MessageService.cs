using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
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
			Match Match = new Match().Get(Request.matchID);

			IQueryable<Message> Message = new BaseRepository<Message> ().List (x => 
				((x.FromUser == Match.FirstUser && x.ToUser == Match.SecondUser) ||
				(x.FromUser == Match.SecondUser && x.ToUser == Match.FirstUser)) &&
				x.Created <= DateTime.ParseExact(Request.date, "yyyy-MM-dd'T'HH:mm:ss'GTM'zzz", CultureInfo.InvariantCulture)
			);

			if (Request.limit > 0)
			{
				Message = Message.Take(Request.limit);
			}

			return new GetMessagesResponse(Message.ToArray());
		}

		public object Post(PostMessageRequest Request)
		{
			Message Message = Request.GetEntity();
			Message.FromUser = GetAuthenticatedUser();

			Message.Save();

			return new BaseResponse();
		}
	}
}
