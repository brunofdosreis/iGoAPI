using System;

namespace iGO.Domain.Entities
{
	public abstract class BaseEntity
	{
		public virtual int Id { get;  set; }
		public virtual DateTime Created { get; protected set; }
		public virtual DateTime Updated { get; set; }

		protected BaseEntity()
		{
			Created = DateTime.Now;
		}
	}

	public abstract class BaseEntity<T> : BaseEntity
	{
	}
}
