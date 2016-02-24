using System;

namespace iGO.Domain.Entities
{
	public class Hello : BaseEntity<Hello>
	{
		public virtual string Name { get; set; }
	}
}
