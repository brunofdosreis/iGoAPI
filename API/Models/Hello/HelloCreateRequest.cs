using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/hello/create", "POST")]
	public class HelloCreateRequest : BaseRequest<object>, IReturn<BaseResponse>
	{
		public Object[] user { get; set; }

		public override object GetEntity(NHibernate.ISession session)
		{
			return null;
		}

		public class Object
		{
			public string facebookToken { get; set; }
			public int ageStart { get; set; }
			public int ageEnd { get; set; }
			public string[] gender { get; set; }
		}
	}
}
