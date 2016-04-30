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
			Map(x => x.Updated);
			Map(x => x.FacebookId);
			Map(x => x.Title);
			Map(x => x.Description).CustomSqlType("nvarchar (4000)");
			Map(x => x.StartDate);
			Map(x => x.EndDate);

			HasManyToMany(x => x.User).Table("UserEvent").Cascade.All();
		}
	}
}
