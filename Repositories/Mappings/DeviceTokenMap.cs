using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class DeviceTokenMap : ClassMap<DeviceToken>
	{
		public DeviceTokenMap()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.Token);
			Map(x => x.Platform);

			References(x => x.User);
		}
	}
}
