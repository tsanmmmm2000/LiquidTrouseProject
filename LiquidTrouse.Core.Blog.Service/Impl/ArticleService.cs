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
                MinusTagUsedCount(originalTag);
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

        private string[] FilterDuplicatedTags(string[] displayNames)
		{
            if (displayNames == null)
            {
                displayNames = new string[] { };
            }
            var result = new List<string>();
            foreach (var displayName in displayNames)
            {
                if (!result.Contains(displayName.Trim().ToLower()))
                {
                    result.Add(displayName.Trim().ToLower());
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
            IList finalTags = new List<Tag>();
            for (int i = 0; i < newTagDisplayNames.Count; i++)
            {
                var newTagDisplayName = newTagDisplayNames[i];
                newTagDisplayName = newTagDisplayName.ToLower().Trim();

                if (!String.IsNullOrEmpty(newTagDisplayName))
                {
                    for (int k = 0; k < originalTags.Count; k++)
                    {
                        var originalTag = originalTags[k] as Tag;
                        if (originalTag.DisplayName.ToLower().Trim().Equals(newTagDisplayName))
                        {
                            originalTags.RemoveAt(k);
                            newTagDisplayNames.RemoveAt(i);
                            finalTags.Add(originalTag);
                            i--;
                            k--;
                            break;
                        }
                    }
                }
            }

            foreach (Tag originalTag in originalTags)
            {
                MinusTagUsedCount(originalTag);
            }

            IList temp = null;
            if (newTagDisplayNames != null)
            {
                if (newTagDisplayNames.Count == 0)
                {
                    if (tagDisplayNames != null && tagDisplayNames.Length > 0)
                    {
                        temp = _tagDao.GetByName(new List<string>(tagDisplayNames));
                    }
                }
                else
                {
                    string[] tempArray = newTagDisplayNames.ToArray();
                    temp = EnsureTagsCreated(tempArray);
                }
            }

            if (temp != null)
            {
                foreach (Tag tag in temp)
                {
                    if (!finalTags.Contains(tag))
                    {
                        finalTags.Add(tag);
                    }
                }
            }
            return finalTags;
        }
        private IList EnsureTagsCreated(string[] displayNames)
		{
			var tags = new List<Tag>();
            var allExistedTags = _tagDao.GetByName(new List<string>(displayNames));

            foreach (var displayName in displayNames)
            {
                if (!String.IsNullOrEmpty(displayName))
                {
                    bool needCreation = true;
                    foreach (Tag tag in allExistedTags)
                    {
                        if (tag.DisplayName.Trim().Equals(displayName.ToLower().Trim()))
                        {
                            needCreation = false;
                            Tag existedTag = null;
                            existedTag = AddTagUsedCount(tag);
                            tags.Add(existedTag);
                            break;
                        }
                    }
                    if (needCreation)
                    {
                        tags.Add(CreateTag(displayName));
                    }
                }
            }
            return tags;
		}	
		private Tag CreateTag(string displayName)
        {
            Tag newTag = new Tag();
            newTag.DisplayName = displayName.ToLower().Trim();
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
        private Tag AddTagUsedCount(Tag tag)
        {
            tag.UsedCount += 1;
            tag.LastUsedDatetime = DateTime.UtcNow;
            _tagDao.Update(tag);
            return tag;
        }	
        private void MinusTagUsedCount(Tag tag)
        {
            tag.UsedCount -= 1;
            if (tag.UsedCount < 0)
            {
                tag.UsedCount = 0;
            }
            _tagDao.Update(tag);
        }
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
    }
}
