using LiquidTrouse.Core.Blog.DataAccess.Domain;
using NHibernate;
using Spring.Data.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess.Impl.NHibernate
{
    public class NHibernateHitDao : IHitDao
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

        public Hit GetLatestByIPAddress(string ipAddress, int resourceId, HitType hitType)
        {
            var hql = "select h from Hit as h where h.IPAddress=? and h.ResourceId=? and h.HitType=? order by h.CreationDatetime desc";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, ipAddress);
            query.SetParameter(1, resourceId);
            query.SetParameter(2, hitType);
            query.SetMaxResults(1);
            var result = query.UniqueResult() as Hit;
            return result;
        }
        public IList GetTopN(int topN, HitType hitType)
        {
            var hql = "select h from Hit as h where h.HitType=? group by h.ResourceId order by count(h.ResourceId) desc";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, hitType);
            query.SetMaxResults(topN);
            IList result = query.List();
            return result;
        }
        public int GetCount(int resourceId, HitType hitType)
        {
            var hql = "select count(h.ResourceId) from Hit as h where h.ResourceId=? and h.HitType=?";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, resourceId);
            query.SetParameter(1, hitType);
            var result = query.UniqueResult();
            return (result != null) ? Convert.ToInt32(result) : 0;
        }
        public void Create(Hit hit)
        {
            var session = GetSession();
            session.Save(hit);
        }
        public void Update(Hit hit)
        {
            var session = GetSession();
            session.SaveOrUpdate(hit);
        }
        public void Delete(int resourceId, HitType hitType)
        {
            var hql = "delete Hit as h where h.ResourceId=? and h.HitType=?";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, resourceId);
            query.SetParameter(1, hitType);
            query.ExecuteUpdate();
        }
    }
}
