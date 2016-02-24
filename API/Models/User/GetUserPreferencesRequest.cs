﻿using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/user/preferences")]
	public class GetUserPreferencesRequest : BaseRequest<Object>, IReturn<GetUserPreferencesResponse>
	{
		public override Object GetEntity()
		{
			return null;
		}
	}
}
