using System;
using System.Collections.Generic;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetUserPicturesResponse : BaseResponse
	{
		public new Object data { get; set; }

		public GetUserPicturesResponse(string[] pictures) : base()
		{
			this.data = new Object(pictures);
		}

		public class Object
		{
			public IEnumerable<Picture> pictures { get; set; }

			public Object(string[] pictures_)
			{
				List<Picture> _pictures = new List<Picture>();

				foreach(string picture in pictures_)
				{
					_pictures.Add(new Picture()
						{
							picture = picture
						}
					);
				}

				pictures = _pictures;
			}

			public class Picture
			{
				public string picture { get; set; }
			}
		}
	}
}
