using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core;
using Spring.Context;

namespace LiquidTrouse.Core.Blog.Service
{
    public class RepositoryFactory
    {
        public RepositoryFactory() { }

        public RepositoryFactory(IApplicationContext context)
        {
            Utility.ApplicationContext = context;
        }

        public IArticleService GetArticleService()
        {
            return Utility.ApplicationContext["ArticleServiceProxy"] as IArticleService;
        }

        public ITagService GetTagService()
        {
            return Utility.ApplicationContext["TagServiceProxy"] as ITagService;
        }

        public ISearchService GetSearchService()
        {
            return Utility.ApplicationContext["SearchServiceProxy"] as ISearchService;
        }

        public IHitService GetHitService()
        {
            return Utility.ApplicationContext["HitServiceProxy"] as IHitService;
        }
    }
}
