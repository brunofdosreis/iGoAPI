using System.Data;
using System.Reflection;
using System.Configuration;

using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;

namespace iGO.Repositories.Configuration
{
	public class NhibernateManager
	{
		public ISessionFactory SessionFactory { get; set; }

		//private ISession Session { get; set; }

		public void CreateDatabase()
		{
			GetSession().CreateSQLQuery("DROP DATABASE IF EXISTS `iGO`; CREATE DATABASE `iGO`;").UniqueResult();

			/*if (Session != null)
			{
				Session.Close();

				Session = null;
			}*/

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

		public ISessionFactory GetSessionFactory()
		{
			if (SessionFactory == null)
			{
				SessionFactory = Fluently.Configure()
					.Database(
						MySQLConfiguration.Standard.ConnectionString(
							ConfigurationManager.ConnectionStrings["DataConnectionSting"].ConnectionString
						)
					)
					.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
					.ExposeConfiguration(cfg => cfg.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "180"))
					.BuildSessionFactory();
			}

			return SessionFactory;
		}

		public ISession GetSession()
		{
			/*if (Session == null || !Session.IsOpen)
			{
				Session = */
					return GetSessionFactory().OpenSession();
			/*}

			return Session;*/
		}

		public void CloseSession()
		{
			/*if (Session != null && Session.IsOpen)
			{
				ITransaction Transaction = Session.Transaction;

				if (Transaction != null && Transaction.IsActive)
				{
					Transaction.Commit();
				}

				Session.Close();
			}*/
		}
	}
}
