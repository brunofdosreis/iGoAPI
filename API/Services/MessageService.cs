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
			Match match = new Match().Get(Request.matchID);

			IEnumerable<Message> message = match.Message.Where(x =>
				x.Created >= DateTime.ParseExact(Request.date, "yyyy-MM-dd'T'HH:mm:ss'GTM'zzz", CultureInfo.InvariantCulture)
			);

			if (Request.limit > 0)
			{
				message = message.Take(Request.limit);
			}

			return new GetMessagesResponse(message.ToArray());
		}

		public object Post(PostMessageRequest Request)
		{
			Message message = Request.GetEntity();
			message.FromUser = GetAuthenticatedUser();

			message.Save();

			return new BaseResponse();
		}
	}
}
