using LiquidTrouse.Core.Blog.DataAccess;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.Blog.Service.DTOConverter;
using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager.DTO;

namespace LiquidTrouse.Core.Blog.Service.Impl
{
    public class SearchService : ISearchService
    {
        private ArticleConverter _converter = new ArticleConverter();

        private ISearchDao _searchDao;
        public ISearchDao SearchDao
        {
            set { _searchDao = value; }
        }

        public PageOf<ArticleInfo> Search(UserInfo user, QueryPackage queryPackage, int pageIndex, int pageSize)
        {
            var keyword = queryPackage.QueryString;
            var sortBy = queryPackage.SortBy.ToString();
            var sortOrder = queryPackage.SortOrder.ToString();
            var startDate = queryPackage.StartDate;
            var endDate = queryPackage.EndDate;
            
            var articles = _searchDao.Search(
                keyword,
                sortBy,
                sortOrder,
                startDate,
                endDate,
                pageIndex, 
                pageSize);

            var articleInfos = _converter.ToDataTransferObject(articles);
            return new PageOf<ArticleInfo>()
            {
                TotalCount = _searchDao.GetTotalCount(keyword, sortBy, startDate, endDate),
                PageCount = pageSize,
                PageNumber = pageIndex,
                PageOfResults = articleInfos
            };
        }

    }
}
