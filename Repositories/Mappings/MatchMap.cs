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
			Map(x => x.Updated);
			Map(x => x.IsFirstUserLike);
			Map(x => x.IsSecondUserLike);
			Map(x => x.DateFirstUser);
			Map(x => x.DateSecondUser);

			References(x => x.FirstUser).Cascade.All();
			References(x => x.SecondUser).Cascade.All();
			References(x => x.Event).Cascade.All();

			HasMany(x => x.Message).Cascade.All();
		}
	}
}
