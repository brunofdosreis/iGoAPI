using System;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class HelloResponse : BaseResponse
	{
		public new Object data;

		public HelloResponse(Hello Hello) : base()
		{
			this.data = new Object(Hello);
		}

		public class Object
		{
			public string name { get; set; }

			public Object(Hello Hello)
			{
				name = Hello.Name;
			}
		}
	}
}
