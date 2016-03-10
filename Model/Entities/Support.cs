using System;

namespace iGO.Domain.Entities
{
	public class Support : BaseEntity<Support>
	{
		public virtual string Platform { get; set; }
		public virtual string Version { get; set; }
		public virtual string Type { get; set; }
	}
}
