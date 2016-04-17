using System;
using System.Collections.Generic;

namespace iGO.Domain.Entities
{
	public class User : BaseEntity<User>
	{
		public virtual ulong FacebookId { get; set; }
		public virtual string FacebookToken { get; set; }
		public virtual string Email { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime Birthday { get; set; }
		public virtual string Gender { get; set; }
		public virtual UserPreferences UserPreferences { get; set; }
		public virtual IEnumerable<UserPictures> UserPictures { get; set; }
		public virtual IEnumerable<DeviceToken> DeviceToken { get; set; }
		public virtual IEnumerable<Event> Event { get; set; }
		public virtual IEnumerable<Match> Match { get; set; }
	}
}
