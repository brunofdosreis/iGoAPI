using System;
using System.Collections.Generic;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class SupportResponse : BaseResponse
	{
		public new Object data { get; set; }

		public SupportResponse(IEnumerable<Support> Support) : base()
		{
			this.data = new Object(Support);
		}

		public class Object
		{
			public IEnumerable<Support> support { get; set; }

			public Object(IEnumerable<iGO.Domain.Entities.Support> Support)
			{
				List<Support> _support = new List<Support>();

				foreach(iGO.Domain.Entities.Support _Support in Support)
				{
					Support s = new Support()
					{
						platform = _Support.Platform,
						version = _Support.Version,
						type = _Support.Type
					};

					_support.Add(s);
				}

				support = _support;
			}

			public class Support
			{
				public string platform { get; set; }
				public string version { get; set; }
				public string type { get; set; }
			}
		}
	}
}
