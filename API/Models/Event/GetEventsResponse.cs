﻿using System;
using System.Collections.Generic;

using iGO.Domain.Entities;

namespace iGO.API.Models
{
	public class GetEventsResponse : BaseResponse
	{
		public new Object data;

		public GetEventsResponse (IEnumerable<Event> Events) : base()
		{
			this.data = new Object(Events);
		}

		public class Object
		{
			public IEnumerable<Event> events { get; set; }

			public Object(IEnumerable<iGO.Domain.Entities.Event> Events)
			{
				List<Event> _events = new List<Event>();

				foreach(iGO.Domain.Entities.Event _event in Events)
				{
					_events.Add(new Event()
						{
							ID = _event.Id,
							title = _event.Title,
							desciption = _event.Description
						}
					);
				}

				events = _events;
			}

			public class Event
			{
				public int ID { get; set; }
				public string title { get; set; }
				public string desciption { get; set; }
			}
		}
	}
}

