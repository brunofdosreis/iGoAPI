using System;

namespace iGO.Domain.Entities
{
	public class UserPictures : BaseEntity<UserPictures>
	{
		public virtual string Picture { get; set; }
		public virtual bool IsDefault { get; set; }
		public virtual User User { get; set; }
	}
}
