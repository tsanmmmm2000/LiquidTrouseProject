using LiquidTrouse.Core.Blog.DataAccess.Domain;
using NHibernate;
using Spring.Data.NHibernate;
using System;
using System.Collections;

namespace LiquidTrouse.Core.Blog.DataAccess.Impl.NHibernate
{
    public class NHibernateArticleDao : IArticleDao
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

        public Article Get(int articleId)
        {
            var hql = "select a from Article as a where a.ArticleId=? and a.Status=0";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, articleId);
            var result = query.UniqueResult() as Article;
            return result;
        }
        public IList Get(int pageIndex, int pageSize)
        {
            var offset = pageIndex * pageSize;
            var hql = "select a from Article as a where a.Status=0 order by a.CreationDatetime desc";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetFirstResult(offset);
            query.SetMaxResults(pageSize);
            IList result = query.List();
            return result;
        }
        public int GetTotalCount()
        {
            var hql = "select count(a.ArticleId) from Article as a where a.Status=0";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            var result = query.UniqueResult();
            return (result != null) ? Convert.ToInt32(result) : 0;
        }
        public void Create(Article article)
        {
            var session = GetSession();
            session.Save(article);
        }
        public void Update(Article article)
        {
            var session = GetSession();
            session.SaveOrUpdate(article);
        }
        public void Delete(int articleId)
        {
            var session = GetSession();
            var article = (Article)session.Load(typeof(Article), articleId);
            session.Delete(article);
        }
    }
}
