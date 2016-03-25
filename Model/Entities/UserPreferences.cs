using System;
using System.Collections.Generic;

namespace iGO.Domain.Entities
{
	public class UserPreferences : BaseEntity<UserPreferences>
	{
		public virtual int AgeStart { get; set; }
		public virtual int AgeEnd { get; set; }
		public virtual IEnumerable<string> Gender { get; set; }
		public virtual User User { get; set; }
	}
}
