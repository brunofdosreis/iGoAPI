using System;

namespace iGO.API.Models
{
	public abstract class BaseRequest<T>
	{
		public BaseRequest()
		{
		}

		public abstract T GetEntity();
	}
}
