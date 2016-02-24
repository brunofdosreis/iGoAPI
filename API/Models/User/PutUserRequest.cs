using System;

using ServiceStack;

using iGO.Domain.Entities;
using iGO.Repositories.Extensions;

namespace iGO.API.Models
{
	[Route("/user")]
	public class PutUserRequest : BaseRequest<UserPictures>, IReturn<BaseResponse>
	{
		public int defaultPictureID { get; set; }

		public override UserPictures GetEntity()
		{
			UserPictures Picture = new UserPictures().Get(defaultPictureID);

			return Picture;
		}
	}
}
