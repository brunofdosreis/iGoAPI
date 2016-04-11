using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class PostMessageResponse : BaseResponse
	{
		public new Object data { get; set; }

		public class Object
		{
		}
	}
}
