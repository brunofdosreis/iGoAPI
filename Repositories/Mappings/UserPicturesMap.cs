using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class UserPicturesMap : ClassMap<UserPictures>
	{
		public UserPicturesMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.Updated);
			Map(x => x.Picture);
			Map(x => x.IsDefault);

			References(x => x.User).Cascade.All();
		}
	}
}
