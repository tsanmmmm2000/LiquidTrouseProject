using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager.DTO;

namespace LiquidTrouse.Core.Blog.Service
{
    public interface ISearchService
    {
        PageOf<ArticleInfo> Search(UserInfo user, QueryPackage queryPackage, int pageIndex, int pageSize);
    }
}
