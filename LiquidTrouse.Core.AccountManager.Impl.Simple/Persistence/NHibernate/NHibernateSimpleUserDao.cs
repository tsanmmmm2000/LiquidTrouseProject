using LiquidTrouse.Core.AccountManager.Impl.Simple.Domain;
using NHibernate;
using Spring.Data.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager.Impl.Simple.Persistence.NHibernate
{
    public class NHibernateSimpleUserDao : ISimpleUserDao
    {
        private ISessionFactory sessionFactory;
        public ISessionFactory SessionFactory
        {
            set { sessionFactory = value; }
        }

        private ISession GetSession()
        {
            return SessionFactoryUtils.GetSession(sessionFactory, true);
        }

        public IList GetAll()
        {
            var hql = "select su from SimpleUser as su";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            IList result = query.List();
            return result;
        }
        public SimpleUser GetByUserId(string userId)
        {
            var hql = "select su from SimpleUser as su where su.UserId=?";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, userId);
            var result = query.UniqueResult() as SimpleUser;
            return result;
        }
        public SimpleUser GetByLoginId(string loginId)
        {
            var hql = "select su from SimpleUser as su where su.LoginId=?";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, loginId);
            var result = query.UniqueResult() as SimpleUser;
            return result;
        }
        public void Create(SimpleUser user)
        {
            var session = GetSession();
            session.Save(user);
        }
        public void Update(SimpleUser user)
        {
            var session = GetSession();
            session.SaveOrUpdate(user);
        }
        public void Delete(string userId)
        {
            var session = GetSession();
            var user = (SimpleUser)session.Load(typeof(SimpleUser), userId);
            session.Delete(user);
        }
    }
}
