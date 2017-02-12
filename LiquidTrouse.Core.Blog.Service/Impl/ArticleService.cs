using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager;
using LiquidTrouse.Core.AccountManager.DTO;
using LiquidTrouse.Core.Blog.DataAccess;
using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.Blog.Service.DTOConverter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiquidTrouse.Core.Blog.Service.Impl
{
    public class ArticleService : IArticleService
    {
        private ArticleConverter _articleConverter = new ArticleConverter();
        private SortingConverter _sortingConverter = new SortingConverter();

        private IArticleDao _articleDao;
        public IArticleDao ArticleDao
        {
            set { _articleDao = value; }
        }
		
        private ITagDao _tagDao;
        public ITagDao TagDao
        {
            set { _tagDao = value; }
        }

        private IHitDao _hitDao;
        public IHitDao HitDao
        {
            set { _hitDao = value; }
        }

        public ArticleInfo[] GetTopN(UserInfo userInfo, int topN)
        {
            var articleIdList = _hitDao.GetResourceIds(0, topN, HitType.Article);
            var articleIds = articleIdList.Cast<int>().ToList();
            var articles = _articleDao.Get(articleIds);
            var sortedArticles = SortArticles(articles, articleIds);
            return _articleConverter.ToDataTransferObject(sortedArticles);
        }
        public ArticleInfo Get(UserInfo userInfo, int articleId)
        {
            var article = _articleDao.Get(articleId);
            return _articleConverter.ToDataTransferObject(article);
        }
        public PageOf<ArticleInfo> Get(UserInfo userInfo, int pageIndex, int pageSize)
        {
            return Get(userInfo, new SortingInfo(), pageIndex, pageSize);
        }
        public PageOf<ArticleInfo> Get(UserInfo userInfo, SortingInfo sortingInfo, int pageIndex, int pageSize)
        {
            var sorting = _sortingConverter.ToDomainObject(sortingInfo);

            var actionMethod = this.GetType().GetMethod("GetSortBy" + sorting.SortBy,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var articles = (actionMethod != null)
                ? actionMethod.Invoke(this, new object[] { pageIndex, pageSize, sorting }) as IList
                : GetSortBy(pageIndex, pageSize, sorting);

            var articleInfos = _articleConverter.ToDataTransferObject(articles);
            return new PageOf<ArticleInfo>()
            {
                TotalCount = _articleDao.GetTotalCount(),
                PageCount = pageSize,
                PageNumber = pageIndex,
                PageOfResults = articleInfos
            };
        }
        public void Create(UserInfo userInfo, ArticleInfo articleInfo)
        {
            Create(userInfo, articleInfo, new string[] { });
        }
        public void Create(UserInfo userInfo, ArticleInfo articleInfo, string[] tagDisplayNames)
        {
            CheckAvailable(userInfo);
            tagDisplayNames = FilterDuplicatedTags(tagDisplayNames);
            var tags = EnsureTagsCreated(tagDisplayNames);
            var article = _articleConverter.ToDomainObject(articleInfo);
            article.Tags = tags;
            _articleDao.Create(article);
        }
        public void Delete(UserInfo userInfo, int articleId)
        {
            CheckAvailable(userInfo);
            var article = _articleDao.Get(articleId);
            IList originalTags = article.Tags;
            foreach (Tag originalTag in originalTags)
            {
                SubtractTagUsedCount(originalTag);
            }
            article.Status = ArticleStatus.InActive;
            article.Tags = new ArrayList();
            _articleDao.Update(article);

            _hitDao.Delete(article.ArticleId, HitType.Article);
        }
        public void Update(UserInfo userInfo, ArticleInfo articleInfo)
        {
            Update(userInfo, articleInfo, new string[] { });
        }
        public void Update(UserInfo userInfo, ArticleInfo articleInfo, string[] tagDisplayNames)
        {
            CheckAvailable(userInfo);
            var article = _articleDao.Get(articleInfo.ArticleId);
            article.UserId = articleInfo.UserId;
            article.LastModifiedDatetime = DateTime.UtcNow;
            article.CreationDatetime = articleInfo.CreationDatetime;
            article.Title = articleInfo.Title;
            article.Content = articleInfo.Content;
            article.UrlTitle = articleInfo.UrlTitle;
            article.CoverImageUrl = articleInfo.CoverImageUrl;

            article.Tags = FindFinalTags(article.Tags, FilterDuplicatedTags(tagDisplayNames));
            
            _articleDao.Update(article);
        }

        #region tag related
        private string[] FilterDuplicatedTags(string[] tagDisplayNames)
		{
            if (tagDisplayNames == null)
            {
                tagDisplayNames = new string[] { };
            }
            var result = new List<string>();
            foreach (var tagDisplayName in tagDisplayNames)
            {
                if (!result.Contains(tagDisplayName.Trim().ToLower()))
                {
                    result.Add(tagDisplayName.Trim().ToLower());
                }
            }
            if (result != null && result.Count > 0)
            {
                return result.ToArray() as string[];
            }
            return new string[] { };
		}
        private IList FindFinalTags(IList originalTags, string[] tagDisplayNames)
        {
            var newTagDisplayNames = new List<string>(tagDisplayNames);

            var originalTagsReadyToSubtracted = originalTags.Cast<Tag>().ToList();
            var newTagDisplayNamesReadyToCreated = new List<string>(newTagDisplayNames);
            var finalTags = new List<Tag>();

            foreach (var newTagDisplayName in newTagDisplayNames)
            {
                if (!String.IsNullOrEmpty(newTagDisplayName))
                {
                    foreach(Tag originalTag in originalTags)
                    {
                        if (originalTag.DisplayName.ToLower().Trim().Equals(newTagDisplayName.ToLower().Trim()))
                        {
                            finalTags.Add(originalTag);
                            newTagDisplayNamesReadyToCreated.Remove(newTagDisplayName);
                            originalTagsReadyToSubtracted.Remove(originalTag);
                        }
                    }
                }
            }

            foreach (var originalTag in originalTagsReadyToSubtracted)
            {
                SubtractTagUsedCount(originalTag);
            }

            IList candidate = null;
            if (newTagDisplayNamesReadyToCreated != null)
            {
                if (newTagDisplayNamesReadyToCreated.Count == 0)
                {
                    if (newTagDisplayNames != null && newTagDisplayNames.Count > 0)
                    {
                        candidate = _tagDao.GetByName(newTagDisplayNames);
                    }
                }
                else
                {
                    candidate = EnsureTagsCreated(newTagDisplayNamesReadyToCreated.ToArray());
                }
            }

            if (candidate != null)
            {
                foreach (Tag tag in candidate)
                {
                    if (!finalTags.Contains(tag))
                    {
                        finalTags.Add(tag);
                    }
                }
            }
            return finalTags;
        }
        private IList EnsureTagsCreated(string[] tagDisplayNames)
		{
			var tags = new List<Tag>();
            var allExistedTags = _tagDao.GetByName(new List<string>(tagDisplayNames));

            foreach (var tagDisplayName in tagDisplayNames)
            {
                if (!String.IsNullOrEmpty(tagDisplayName))
                {
                    bool needToCreate = true;
                    foreach (Tag tag in allExistedTags)
                    {
                        if (tag.DisplayName.ToLower().Trim().Equals(tagDisplayName.ToLower().Trim()))
                        {
                            needToCreate = false;
                            AddTagUsedCount(tag);
                            tags.Add(tag);
                            break;
                        }
                    }
                    if (needToCreate)
                    {
                        tags.Add(CreateTag(tagDisplayName));
                    }
                }
            }
            return tags;
		}
        private Tag CreateTag(string tagDisplayName)
        {
            Tag newTag = new Tag();
            newTag.DisplayName = tagDisplayName.ToLower().Trim();
            newTag.UsedCount = 1;
            newTag.LastUsedDatetime = DateTime.UtcNow;
            try
            {
                _tagDao.Create(newTag);
            }
            catch  
            {
                AddTagUsedCount(newTag);
            }
            return newTag;
        }
        private void AddTagUsedCount(Tag tag)
        {
            tag.UsedCount += 1;
            tag.LastUsedDatetime = DateTime.UtcNow;
            _tagDao.Update(tag);
        }	
        private void SubtractTagUsedCount(Tag tag)
        {
            tag.UsedCount -= 1;
            if (tag.UsedCount < 0)
            {
                tag.UsedCount = 0;
            }
            _tagDao.Update(tag);
        }
        #endregion

        #region invoke method
        private IList GetSortByHit(int pageIndex, int pageSize, Sorting sorting)
        {
            var articleIdList = _hitDao.GetResourceIds(pageIndex, pageSize, HitType.Article);
            var articleIds = articleIdList.Cast<int>().ToList();
            var articles = _articleDao.Get(articleIds);
            return SortArticles(articles, articleIds);
        }
        private IList GetSortByCreationDatetime(int pageIndex, int pageSize, Sorting sorting)
        {
            return GetSortBy(pageIndex, pageSize, sorting);
        }
        private IList GetSortByLastModifiedDatetime(int pageIndex, int pageSize, Sorting sorting)
        {
            return GetSortBy(pageIndex, pageSize, sorting);
        }
        private IList GetSortByTitle(int pageIndex, int pageSize, Sorting sorting)
        {
            return GetSortBy(pageIndex, pageSize, sorting);
        }
        private IList GetSortBy(int pageIndex, int pageSize, Sorting sorting)
        {
            return _articleDao.Get(pageIndex, pageSize, sorting);
        }
        #endregion

        #region other
        private void CheckAvailable(UserInfo userInfo)
        {
            var isAdmin = AccountUtility.IsAdminUser(userInfo);
            if (!isAdmin)
            {
                throw new Exception(String.Format("User {0} access denied", userInfo.UserId));
            }
        }
        private IList SortArticles(IList articles, List<int> sortedIds)
        {
            var sortedArticles = new List<Article>();
            foreach (var articleId in sortedIds)
            {
                foreach (Article article in articles)
                {
                    if (article.ArticleId == articleId)
                    {
                        sortedArticles.Add(article);
                        break;
                    }
                }
            }

            foreach (Article article in articles)
            {
                if (!sortedArticles.Contains(article))
                {
                    sortedArticles.Add(article);
                }
            }
            return sortedArticles;
        }
        #endregion
    }
}
