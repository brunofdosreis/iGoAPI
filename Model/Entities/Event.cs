using System;
using System.Collections.Generic;

namespace iGO.Domain.Entities
{
	public class Event : BaseEntity<Event>
	{
		public virtual ulong FacebookId { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual DateTime StartDate { get; set; }
		public virtual DateTime EndDate { get; set; }
		public virtual IEnumerable<User> User { get; set; }
	}
}
