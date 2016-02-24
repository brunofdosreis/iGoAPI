using System;
using System.Collections.Generic;

namespace iGO.Domain.Entities
{
	public class Event : BaseEntity<Event>
	{
		public virtual string FacebookId { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual IEnumerable<User> User { get; set; }
	}
}
