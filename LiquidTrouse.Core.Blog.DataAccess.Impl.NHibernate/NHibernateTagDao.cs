using LiquidTrouse.Core.Blog.DataAccess.Domain;
using NHibernate;
using Spring.Data.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LiquidTrouse.Core.Blog.DataAccess.Impl.NHibernate
{
    public class NHibernateTagDao : ITagDao
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

        public Tag Get(int tagId)
        {
            var session = GetSession();
            var tag = session.Load(typeof(Tag), tagId) as Tag;
            return tag;
        }
        public Tag GetByName(string displayName)
        {
            var hql = "select t from Tag as t where t.DisplayName = ?";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, displayName);
            var result = query.UniqueResult() as Tag;
            return result;
        }
        public IList GetByName(List<string> displayNames)
        {
            var hql = "select t from Tag as t where t.DisplayName in (:displayNameList) order by t.DisplayName";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameterList("displayNameList", displayNames);
            IList result = query.List();
            return result;
        }
        public IList Get(List<int> tagIds)
        {
            var hql = "select t from Tag as t where t.TagId in (:idList) order by t.DisplayName";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameterList("idList", tagIds);
            IList result = query.List();
            return result;
        }
        public IList GetAll()
        {
            var hql = "select t from Tag as t order by t.DisplayName";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            IList result = query.List();
            return result;
        }
        public IList GetEdgeByArticle(int articleId)
        {
            var session = GetSession();
            var hql = "select edge from ArticleTagEdge as edge where edge.ArticleId = ? order by edge.ArticleId";
            var query = session.CreateQuery(hql);
            query.SetParameter(0, articleId);
            IList result = query.List();
            return result;
        }
        public IList GetEdgeByTag(int tagId, int pageIndex, int pageSize)
        {
            var offset = pageIndex * pageSize;
            var hql = "select edge from ArticleTagEdge as edge where edge.TagId = ? order by edge.TagId";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, tagId);
            query.SetFirstResult(offset);
            query.SetMaxResults(pageSize);
            IList result = query.List();
            return result;
        }
        public int GetTotalEdgeCountByTag(int tagId)
        {
            var hql = "select count(edge.ArticleId) from ArticleTagEdge as edge where edge.TagId = ?";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameter(0, tagId);
            var result = query.UniqueResult();
            return (result != null) ? Convert.ToInt32(result) : 0;
        }
        public void Create(Tag tag)
        {
            var session = GetSession();
            session.Save(tag);
        }
        public void Update(Tag tag)
        {
            var session = GetSession();
            session.SaveOrUpdate(tag);
        }
        public void Delete(Tag tag)
        {
            var session = GetSession();
            session.Delete(tag);
        }
        public void Delete(int tagId)
        {
            var session = GetSession();
            var tag = (Tag)session.Load(typeof(Tag), tagId);
            session.Delete(tag);
        }
        public void Delete(List<int> tagIds)
        {
            var hql = "delete Tag as t where t.TagId in (:idList)";
            var session = GetSession();
            var query = session.CreateQuery(hql);
            query.SetParameterList("idList", tagIds);
            query.ExecuteUpdate();
        }
    }
}
