using LiquidTrouse.Core.Blog.DataAccess.Domain;
using System.Collections;
using System.Collections.Generic;

namespace LiquidTrouse.Core.Blog.DataAccess
{
    public interface IArticleDao
    {
        Article Get(int articleId);
        IList Get(List<int> articleIds);
        IList Get(List<int> articleIds, Sorting sorting);
        IList Get(int pageIndex, int pageSize);
        IList Get(int pageIndex, int pageSize, Sorting sorting);
        int GetTotalCount();
        void Create(Article article);
        void Update(Article article);
        void Delete(int articleId);
    }
}
