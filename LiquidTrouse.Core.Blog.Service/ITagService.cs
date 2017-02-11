using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service
{
    public interface ITagService
    {
        TagInfo Get(UserInfo userInfo, int tagId);
        TagInfo GetByName(UserInfo userInfo, string displayName);
        TagInfo[] GetTopN(UserInfo userInfo, int topN);
        TagInfo[] GetByArticle(UserInfo userInfo, int articleId);
        TagInfo[] GetAll(UserInfo userInfo);
        PageOf<ArticleInfo> GetArticlesByTag(UserInfo userInfo, int tagId, int pageIndex, int pageSize);
        void Create(UserInfo userInfo, TagInfo tagInfo);
        void Delete(UserInfo userInfo, int tagId);
        void Update(UserInfo userInfo, TagInfo tagInfo);
    }
}
