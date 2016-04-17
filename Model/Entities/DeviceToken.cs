using System;

namespace iGO.Domain.Entities
{
	public class DeviceToken : BaseEntity<DeviceToken>
	{
		public virtual string Token { get; set; }
		public virtual string Platform { get; set; }
		public virtual User User { get; set; }
	}
}
