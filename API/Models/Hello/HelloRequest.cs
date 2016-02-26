﻿using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/hello")]
	public class HelloRequest : BaseRequest<Hello>, IReturn<HelloResponse>
	{
		public string name { get; set; }

		public override Hello GetEntity()
		{
			Hello Hello = new Hello();

			Hello.Name = name;

			return Hello;
		}
	}
}