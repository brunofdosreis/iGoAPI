using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class EventMap : ClassMap<Event>
	{
		public EventMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.FacebookId);
			Map(x => x.Title);
			Map(x => x.Description);
			Map(x => x.Date);

			HasMany(x => x.User).Cascade.All();
		}
	}
}
