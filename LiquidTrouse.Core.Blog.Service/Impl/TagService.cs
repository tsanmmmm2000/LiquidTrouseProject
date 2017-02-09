using LiquidTrouse.Core.Blog.DataAccess;
using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.Blog.Service.DTOConverter;
using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.Impl
{
    public class TagService : ITagService
    {
        private TagConverter _converter = new TagConverter();

        private ITagDao _tagDao;
        public ITagDao TagDao
        {
            set { _tagDao = value; }
        }

        private IArticleDao _articleDao;
        public IArticleDao ArticleDao
        {
            set { _articleDao = value; }
        }

        public TagInfo Get(UserInfo userInfo, int tagId)
        {
            var tag = _tagDao.Get(tagId);
            return _converter.ToDataTransferObject(tag);
        }
        public TagInfo GetByName(UserInfo userInfo, string displayName)
        {
            displayName = FormatDisplayName(displayName);
            var tag = _tagDao.GetByName(displayName);
            return _converter.ToDataTransferObject(tag);
        }
        public TagInfo[] GetByArticle(UserInfo userInfo, int articleId)
        {
            var tags = GetTagsByEdge(articleId);
            return _converter.ToDataTransferObject(tags);
        }
        public TagInfo[] GetAll(UserInfo userInfo)
        {
            var tags = _tagDao.GetAll();
            return _converter.ToDataTransferObject(tags);
        }
        public PageOf<ArticleInfo> GetArticlesByTag(UserInfo userInfo, int tagId, int pageIndex, int pageSize)
        {
            var articles = GetArticlesByEdge(tagId, pageIndex, pageSize);
            var articleInfos = new ArticleConverter().ToDataTransferObject(articles);
            return new PageOf<ArticleInfo>()
            {
                TotalCount = _tagDao.GetTotalEdgeCountByTag(tagId),
                PageCount = pageSize,
                PageNumber = pageIndex,
                PageOfResults = articleInfos
            };
        }
        public void Create(UserInfo userInfo, TagInfo tagInfo)
        {
            var displayName = FormatDisplayName(tagInfo.DisplayName);
			var tag = _tagDao.GetByName(displayName);
			if (tag == null)
			{
				tag = _converter.ToDomainObject(tagInfo);
				_tagDao.Create(tag);
			}
        }
        public void Delete(UserInfo userInfo, int tagId)
        {
            _tagDao.Delete(tagId);
        }
        public void Update(UserInfo userInfo, TagInfo tagInfo)
        {
            var tag = _tagDao.Get(tagInfo.TagId);
            tag.DisplayName = tagInfo.DisplayName;
            tag.LastUsedDatetime = tagInfo.LastUsedDatetime;
            tag.UsedCount = tagInfo.UsedCount;
            _tagDao.Update(tag);
        }
		
		private string FormatDisplayName(string displayName)
		{
            return displayName.ToLower().Trim();
		}
        private IList GetTagsByEdge(int articleId)
        {
            var tagIdList = new List<int>();
            var edges = _tagDao.GetEdgeByArticle(articleId);
            foreach(ArticleTagEdge edge in edges)
            {
                var tagId = edge.TagId;
                tagIdList.Add(tagId);
            }

            IList tags = new List<Tag>();
            if (tagIdList.Count > 0)
            {
                tags = _tagDao.Get(tagIdList);
            }
            return tags;
        }
        private IList GetArticlesByEdge(int tagId, int pageIndex, int pageSize)
        {
            IList articles = new List<Article>();
            var edges = _tagDao.GetEdgeByTag(tagId, pageIndex, pageSize);
            foreach (ArticleTagEdge edge in edges)
            {
                var articleId = edge.ArticleId;
                var article = _articleDao.Get(articleId);
                articles.Add(article);
            }
            return articles;
        }       
    }
}
