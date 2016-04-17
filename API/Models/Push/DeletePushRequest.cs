using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/push")]
	public class DeletePushRequest : BaseRequest<Object>, IReturn<BaseResponse>
	{
		public string platform { get; set; }
		public string token { get; set; }

		public override Object GetEntity()
		{
			return null;
		}
	}
}
