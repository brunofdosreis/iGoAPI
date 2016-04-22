using System.Data;
using System.Reflection;
using System.Configuration;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;

namespace iGO.Repositories.Configuration
{
	public class NhibernateManager
	{
		private static ISession Session { get; set; }

		public static void CreateDatabase()
		{
			GetSession().CreateSQLQuery("DROP DATABASE IF EXISTS `iGO`; CREATE DATABASE `iGO`;").UniqueResult();

			if (Session != null)
			{
				Session.Close();

				Session = null;
			}

			Fluently.Configure()
				.Database(
					MySQLConfiguration.Standard.ShowSql().ConnectionString(
						ConfigurationManager.ConnectionStrings["DataConnectionSting"].ConnectionString
					)
				)
				.Cache(x => x.Not.UseSecondLevelCache())
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
				.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
				.BuildSessionFactory();
		}

		public static ISession GetSession()
		{
			if (Session == null || !Session.IsOpen)
			{
				Session = Fluently.Configure()
					.Database(
						MySQLConfiguration.Standard.ConnectionString(
							ConfigurationManager.ConnectionStrings["DataConnectionSting"].ConnectionString
						)
					)
					.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
					.ExposeConfiguration(cfg => cfg.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "180"))
					.BuildSessionFactory()
					.OpenSession();
			}

			return Session;
		}
	}
}
