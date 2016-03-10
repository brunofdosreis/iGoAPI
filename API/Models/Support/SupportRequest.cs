using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/support")]
	public class SupportRequest : BaseRequest<Support>, IReturn<SupportResponse>
	{
		public string platform { get; set; }

		public override Support GetEntity()
		{
			Support Support = new Support();

			Support.Platform = platform;

			return Support;
		}
	}
}
