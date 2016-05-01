using System;

namespace iGO.API.Models
{
	public abstract class BaseRequest<T> : BaseRequest
	{
		public virtual string Version { get; set; }

		public abstract T GetEntity(NHibernate.ISession session);
	}

	public interface BaseRequest
	{
		string Version { get; set; }
	}
}
