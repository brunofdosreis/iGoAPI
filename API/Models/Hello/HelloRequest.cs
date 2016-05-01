using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/{Version}/hello")]
	public class HelloRequest : BaseRequest<Hello>, IReturn<HelloResponse>
	{
		public override string Version
		{
			get
			{
				return null;
			}
		}

		public string name { get; set; }

		public override Hello GetEntity(NHibernate.ISession session)
		{
			Hello Hello = new Hello();

			Hello.Name = name;

			return Hello;
		}
	}
}
