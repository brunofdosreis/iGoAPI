using System;

namespace iGO.Domain.Entities
{
	public class Message : BaseEntity<Message>
	{
		public virtual string Text { get; set; }
		public virtual Match Match { get; set; }
		public virtual User FromUser { get; set; }
		public virtual User ToUser { get; set; }
	}
}
