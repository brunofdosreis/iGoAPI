using System;

namespace iGO.Domain.Entities
{
	public class UserPreferences : BaseEntity<UserPreferences>
	{
		public virtual int AgeStart { get; set; }
		public virtual int AgeEnd { get; set; }
		public virtual string Gender { get; set; }
		public virtual User User { get; set; }
	}
}
