using System;

namespace iGO.API.Models
{
	public class BaseResponse
	{
		public int status { get; set; }
		public string message { get; set; }
		public virtual Object data { get; set; }

		public BaseResponse() {

			this.status = 200;
			this.message = "";
		}
	}
}
