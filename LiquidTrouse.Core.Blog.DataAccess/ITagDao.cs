using LiquidTrouse.Core.Blog.DataAccess.Domain;
using System.Collections;
using System.Collections.Generic;

namespace LiquidTrouse.Core.Blog.DataAccess
{
    public interface ITagDao
    {
        Tag Get(int tagId);
        Tag GetByName(string displayName);
        IList GetByName(List<string> displayNames);
        IList Get(List<int> tagIds);
        IList GetAll();
        IList GetEdgeByArticle(int articleId);
        IList GetEdgeByTag(int tagId, int pageIndex, int pageSize);
        int GetTotalEdgeCountByTag(int tagId);
        void Create(Tag tag);
        void Update(Tag tag);
        void Delete(Tag tag);
        void Delete(int tagId);
        void Delete(List<int> tagIds);
    }
}
