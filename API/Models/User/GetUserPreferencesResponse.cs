using System;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetUserPreferencesResponse : BaseResponse
	{
		public new Object data { get; set; }

		public GetUserPreferencesResponse(UserPreferences UserPreferences) : base()
		{
			this.data = new Object(UserPreferences);
		}

		public class Object
		{
			public int ageStart { get; set; }
			public int ageEnd { get; set; }

			public Object(UserPreferences UserPreferences)
			{
				ageStart = UserPreferences.AgeStart;
				ageEnd = UserPreferences.AgeEnd;
			}
		}
	}
}
