using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class HelloMap : ClassMap<Hello>
	{
		public HelloMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.Updated);
			Map(x => x.Name);
		}
	}
}
