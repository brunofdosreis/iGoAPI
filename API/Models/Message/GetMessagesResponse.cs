using System;
using System.Collections.Generic;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetMessagesResponse : BaseResponse
	{
		public new Object[] data { get; set; }

		public GetMessagesResponse(Message[] Message)
		{
			data = new Object[] {};

			List<Object> Messages = new List<Object>();

			foreach (Message message in Message)
			{
				Messages.Add (new Object()
					{
						ID = message.Id,
						date = message.Created.ToString("yyyy-MM-dd'T'HH:mm:ss'GMT'zzz"),
						matchID = message.Match.Id,
						fromUserID = message.FromUser.Id,
						toUserID = message.ToUser.Id,
						text = message.Text
					}
				);
			}

			data = Messages.ToArray();
		}

		public class Object
		{
			public int ID { get; set; }
			public string date { get; set; }
			public int matchID { get; set; }
			public int fromUserID { get; set; }
			public int toUserID { get; set; }
			public string text { get; set; }
		}
	}
}
