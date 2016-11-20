using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog.Models;
using System.Web;

namespace SimpleBlog
{
    public class Database
    {
        private const string SessionKey = "SimpleBlog.Database.SessionKey";
        private static ISessionFactory _sessionFactory;
        public static ISession Session => (ISession)HttpContext.Current.Items[SessionKey];
        public static void Configure()
        {
            var config = new Configuration();
            //Configure Connection String
            config.Configure();
            //add out mapping
            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>();
            mapper.AddMapping<RoleMap>();

            mapper.AddMapping<PostMap>();
            mapper.AddMapping<TagMap>();


            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            //create sessin factory
            _sessionFactory = config.BuildSessionFactory();
        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
            {
                session.Close();
                HttpContext.Current.Items.Remove(SessionKey);
            }
        }
    }
}