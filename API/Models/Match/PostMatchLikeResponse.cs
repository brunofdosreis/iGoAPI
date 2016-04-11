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
				isMatch = (Match.IsFirstUserLike ?? false) && 
					(Match.IsSecondUserLike ?? false)
			};
		}

		public class Object
		{
			public bool isMatch { get; set; }
		}
	}
}
