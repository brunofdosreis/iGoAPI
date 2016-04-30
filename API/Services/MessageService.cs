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
			Match match = new Match().Get(Request.matchID);

			IEnumerable<Message> message = new BaseRepository<Message> ().List (x => 
				x.Match == match 
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
			Message message = Request.GetEntity();
			message.FromUser = GetAuthenticatedUser();

			message.Save();

			message.Match.Save();

			User user = message.ToUser;

			List<DeviceToken> deviceToken = new BaseRepository<DeviceToken> ().List (x =>
				x.User.Id == user.Id).ToList();

			if (deviceToken != null && deviceToken.Any ())
			{
				PushHelper.SendNotification(deviceToken, "Nova mensagem de " + message.FromUser.Name, "message");
			}

			return new BaseResponse();
		}
	}
}
