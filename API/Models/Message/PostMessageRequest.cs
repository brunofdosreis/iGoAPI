using System;

using ServiceStack;

using iGO.Domain.Entities;
using iGO.Repositories.Extensions;

namespace iGO.API.Models
{
	[Route("/{Version}/message")]
	public class PostMessageRequest : BaseRequest<Message>, IReturn<BaseResponse>
	{
		public int matchID { get; set; }
		public int toUserID { get; set; }
		public string text { get; set; }

		public override Message GetEntity(NHibernate.ISession session)
		{
			Message Message = new Message();

			Message.Match = new Match().Get(matchID, session);
			Message.ToUser = new User().Get(toUserID, session);
			Message.Text = text;

			return Message;
		}
	}
}
