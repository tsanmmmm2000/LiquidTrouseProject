using LiquidTrouse.Core.Blog.DataAccess.Domain;
using System.Collections;

namespace LiquidTrouse.Core.Blog.DataAccess
{
    public interface IArticleDao
    {
        Article Get(int articleId);
        IList Get(int pageIndex, int pageSize);
        int GetTotalCount();
        void Create(Article article);
        void Update(Article article);
        void Delete(int articleId);
    }
}
