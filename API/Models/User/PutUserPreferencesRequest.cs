using System;

using ServiceStack;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	[Route("/user/preferences")]
	public class PutUserPreferencesRequest : BaseRequest<UserPreferences>, IReturn<BaseResponse>
	{
		public int ageStart { get; set; }
		public int ageEnd { get; set; }
		public string gender { get; set; }

		public override UserPreferences GetEntity()
		{
			UserPreferences UserPreferences = new UserPreferences();

			UserPreferences.AgeStart = ageStart;
			UserPreferences.AgeEnd = ageEnd;
			UserPreferences.Gender = gender;

			return UserPreferences;
		}
	}
}
