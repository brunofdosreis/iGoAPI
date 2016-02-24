using System;

using NHibernate;

using iGO.Domain.Entities;

namespace iGO.Repositories.Extensions
{
	public static class BaseEntityExtension
	{
		public static void Save<T>(this BaseEntity<T> entity)
		{
			new BaseRepository<T>().Save(entity);
		}

		public static void Save<T>(this BaseEntity<T> entity, ISession Session)
		{
			new BaseRepository<T>(Session).Save(entity);
		}

		public static void Delete<T>(this BaseEntity<T> entity)
		{
			new BaseRepository<T>().Delete(entity);
		}

		public static void Delete<T>(this BaseEntity<T> entity, ISession Session)
		{
			new BaseRepository<T>(Session).Delete(entity);
		}

		public static T Get<T>(this BaseEntity<T> entity, int Id)
		{
			return new BaseRepository<T>().Get(Id);
		}

		public static T Get<T>(this BaseEntity<T> entity, int Id, ISession Session)
		{
			return new BaseRepository<T>(Session).Get(Id);
		}
	}
}
