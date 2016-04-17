using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/user/pictures", "PUT")]
	public class PutUserPicturesRequest : BaseRequest<object>, IReturn<BaseResponse>
	{
		public Object[] pictures { get; set; }

		public override object GetEntity()
		{
			return null;
		}

		public class Object
		{
			public int ID { get; set; }
			public string picture { get; set; }
			public bool isDefault { get; set; }
		}
	}
}
