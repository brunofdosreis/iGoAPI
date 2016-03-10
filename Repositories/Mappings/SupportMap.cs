using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class SupportMap : ClassMap<Support>
	{
		public SupportMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.Platform);
			Map(x => x.Version);
			Map(x => x.Type);
		}
	}
}
