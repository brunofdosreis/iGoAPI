using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/push", "PUT")]
	public class PutPushRequest : BaseRequest<DeviceToken>, IReturn<BaseResponse>
	{
		public string platform { get; set; }
		public string token { get; set; }

		public override DeviceToken GetEntity()
		{
			DeviceToken deviceToken = new DeviceToken()
			{
				Platform = platform,
				Token = token
			};

			return deviceToken;
		}
	}
}
