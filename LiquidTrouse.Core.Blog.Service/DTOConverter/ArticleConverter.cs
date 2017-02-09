using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using System.Collections;
using System.Collections.Generic;

namespace LiquidTrouse.Core.Blog.Service.DTOConverter
{
    public class ArticleConverter
    {
        public ArticleConverter() { }

        public IList ToDomainObject(ArticleInfo[] articleInfos)
        {
            var articleList = new List<Article>();
            if (articleInfos != null && articleInfos.Length > 0)
            {
                foreach (var articleInfo in articleInfos)
                {
                    var article = new Article();
                    article.ArticleId = articleInfo.ArticleId;
                    article.UserId = articleInfo.UserId;
                    article.CreationDatetime = articleInfo.CreationDatetime;
                    article.LastModifiedDatetime = articleInfo.LastModifiedDatetime;
                    article.Title = articleInfo.Title;
                    article.Content = articleInfo.Content;
                    article.UrlTitle = articleInfo.UrlTitle;
                    article.CoverImageUrl = articleInfo.CoverImageUrl;
                    articleList.Add(article);
                }
            }
            return articleList;
        }

        public Article ToDomainObject(ArticleInfo articleInfo)
        {
            var article = ToDomainObject(new ArticleInfo[] { articleInfo })[0] as Article;
            return article;
        }

        public ArticleInfo[] ToDataTransferObject(IList articles)
        {
            var articleInfoList = new List<ArticleInfo>();
            if (articles != null && articles.Count > 0)
            {
                foreach (Article article in articles)
                {
                    var articleInfo = new ArticleInfo();
                    articleInfo.ArticleId = article.ArticleId;
                    articleInfo.CreationDatetime = article.CreationDatetime;
                    articleInfo.LastModifiedDatetime = article.LastModifiedDatetime;
                    articleInfo.UserId = article.UserId;
                    articleInfo.Content = article.Content;
                    articleInfo.Title = article.Title;
                    articleInfo.UrlTitle = article.UrlTitle;
                    articleInfo.CoverImageUrl = article.CoverImageUrl;
                    articleInfoList.Add(articleInfo);
                }
            }
            return articleInfoList.ToArray();
        }

        public ArticleInfo ToDataTransferObject(Article article)
        {
            return ToDataTransferObject(new List<Article>() { article })[0];
        }
    }
}
