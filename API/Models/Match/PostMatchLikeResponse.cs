using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class PostMatchLikeResponse : BaseResponse
	{
		public new Object data { get; set; }

		public PostMatchLikeResponse(Match Match)
		{
			data = new Object () {
				matchID = Match.Id,
				isMatch = (Match.IsFirstUserLike) && 
					(Match.IsSecondUserLike ?? false)
			};
		}

		public class Object
		{
			public int matchID { get; set; }
			public bool isMatch { get; set; }
		}
	}
}
