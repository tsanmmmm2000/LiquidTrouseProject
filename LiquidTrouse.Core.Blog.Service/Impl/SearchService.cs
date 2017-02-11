using LiquidTrouse.Core.Blog.DataAccess;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.Blog.Service.DTOConverter;
using LiquidTrouse.Core;
using LiquidTrouse.Core.AccountManager.DTO;

namespace LiquidTrouse.Core.Blog.Service.Impl
{
    public class SearchService : ISearchService
    {
        private ArticleConverter _articleConverter = new ArticleConverter();
        private SortingConverter _sortingConverter = new SortingConverter();

        private ISearchDao _searchDao;
        public ISearchDao SearchDao
        {
            set { _searchDao = value; }
        }

        public PageOf<ArticleInfo> Search(UserInfo user, QueryPackage queryPackage, int pageIndex, int pageSize)
        {
            var keyword = queryPackage.QueryString;
            var sorting = _sortingConverter.ToDomainObject(queryPackage.SortingInfo);
            var startDate = queryPackage.StartDate;
            var endDate = queryPackage.EndDate;
            
            var articles = _searchDao.Search(
                keyword,
                sorting,
                startDate,
                endDate,
                pageIndex, 
                pageSize);

            var articleInfos = _articleConverter.ToDataTransferObject(articles);
            return new PageOf<ArticleInfo>()
            {
                TotalCount = _searchDao.GetTotalCount(keyword, sorting, startDate, endDate),
                PageCount = pageSize,
                PageNumber = pageIndex,
                PageOfResults = articleInfos
            };
        }

    }
}
