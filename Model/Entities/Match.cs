using System;
using System.Collections.Generic;

namespace iGO.Domain.Entities
{
	public class Match : BaseEntity<Match>
	{
		public virtual bool IsFirstUserLike { get; set; }
		public virtual bool? IsSecondUserLike { get; set; }
		public virtual DateTime DateFirstUser { get; set; }
		public virtual DateTime? DateSecondUser { get; set; }
		public virtual User FirstUser { get; set; }
		public virtual User SecondUser { get; set; }
		public virtual Event Event { get; set; }
		public virtual IEnumerable<Message> Message { get; set; }
	}
}
