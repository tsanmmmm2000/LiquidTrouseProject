using NHibernate;
using Spring.Data.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess.Impl.NHibernate
{
    public class NHibernateSearchDao : ISearchDao
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

        public IList Search(
            string keyword,
            string sortBy,
            string sortOrder,
            DateTime? startDate,
            DateTime? endDate,
            int pageIndex,
            int pageSize)
        {
            var offset = pageIndex * pageSize;
            
            var sb = new StringBuilder();
            sb.Append("select a from Article as a where (a.Title like '%{0}%' or a.Content like '%{0}%') and a.Status=0 ");
            var useDate = (startDate != null && endDate != null);
            if (useDate)
            {
                sb.Append("and a.{1} between ? and ? ");
            }
            sb.Append("order by a.{1} {2}");
            
            string hql = String.Format(sb.ToString(), keyword, sortBy, sortOrder);
            
            var session = GetSession();
            var query = session.CreateQuery(hql);
            
            if (useDate)
            {
                query.SetParameter(0, startDate);
                query.SetParameter(1, endDate);
            }

            query.SetFirstResult(offset);
            query.SetMaxResults(pageSize);
            IList result = query.List();
            return result;
        }

        public int GetTotalCount(
            string keyword, 
            string sortBy, 
            DateTime? startDate, 
            DateTime? endDate)
        {
            string hql = string.Empty;
            var sb = new StringBuilder();
            sb.Append("select count(a.ArticleId) from Article as a where (a.Title like '%{0}%' or a.Content like '%{0}%') and a.Status=0 ");
            var useDate = (startDate != null && endDate != null);
            if (useDate)
            {
                sb.Append("and a.{1} between ? and ? ");
                hql = String.Format(sb.ToString(), keyword, sortBy);
            }
            else
            {
                hql = String.Format(sb.ToString(), keyword);
            }
            var session = GetSession();
            var query = session.CreateQuery(hql);
            
            if (useDate)
            {
                query.SetParameter(0, startDate);
                query.SetParameter(1, endDate);
            }

            var result = query.UniqueResult();
            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }

    }
}
