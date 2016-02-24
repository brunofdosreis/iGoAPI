using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class MatchMap : ClassMap<Match>
	{
		public MatchMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.IsMatch);
			Map(x => x.DateFirstUser);
			Map(x => x.DateSecondUser);

			References(x => x.FirstUser);
			References(x => x.SecondUser);
			References(x => x.Event);

			HasMany(x => x.Message);
		}
	}
}
