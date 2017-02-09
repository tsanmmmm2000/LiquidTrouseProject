using LiquidTrouse.Core.Blog.DataAccess;
using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.Blog.Service.DTOConverter;
using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using LiquidTrouse.Core.AccountManager;

namespace LiquidTrouse.Core.Blog.Service.Impl
{
    public class ArticleService : IArticleService
    {
        private ArticleConverter _converter = new ArticleConverter();

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

        public ArticleInfo Get(UserInfo userInfo, int articleId)
        {
            var article = _articleDao.Get(articleId);
            return _converter.ToDataTransferObject(article);
        }
        public PageOf<ArticleInfo> Get(UserInfo userInfo, int pageIndex, int pageSize)
        {
            var articles = _articleDao.Get(pageIndex, pageSize);
            var articleInfos = _converter.ToDataTransferObject(articles);
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
            tagDisplayNames = GetNoDuplicatedTags(tagDisplayNames);
            var tags = EnsureTagsCreated(tagDisplayNames);
            var article = _converter.ToDomainObject(articleInfo);
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

            article.Tags = FindFinalTags(article.Tags, GetNoDuplicatedTags(tagDisplayNames));
            
            _articleDao.Update(article);
        }

        private string[] GetNoDuplicatedTags(string[] displayNames)
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
                            existedTag = UpdateExistedTag(tag);
                            tags.Add(existedTag);
                            break;
                        }
                    }
                    if (needCreation)
                    {
                        tags.Add(CreateNewTag(displayName));
                    }
                }
            }
            return tags;
		}	
		private Tag UpdateExistedTag(Tag tag)
        {
            tag.UsedCount += 1;
            tag.LastUsedDatetime = DateTime.UtcNow;
            _tagDao.Update(tag);
            return tag;
        }	
		private Tag CreateNewTag(string displayName)
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
                UpdateExistedTag(newTag);
            }
            return newTag;
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
    }
}
