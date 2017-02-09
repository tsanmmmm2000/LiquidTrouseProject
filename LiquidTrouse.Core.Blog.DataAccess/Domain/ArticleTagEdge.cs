using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess.Domain
{
    public class ArticleTagEdge
    {
        private int _tagId;
        private int _articleId;


        public int TagId
        {
            get { return _tagId; }
            set { _tagId = value; }
        }

        public int ArticleId
        {
            get { return _articleId; }
            set { _articleId = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }
            if (!(obj is ArticleTagEdge))
            {
                return false;
            }

            ArticleTagEdge edge = (ArticleTagEdge)obj;
            string objKey = edge.TagId.ToString() + "_" + edge.ArticleId.ToString();
            string thisKey = this.TagId.ToString() + "_" + this.ArticleId.ToString();
            return (objKey.Equals(thisKey));

        }

        public override int GetHashCode()
        {
            return this.TagId.GetHashCode() + this.ArticleId.GetHashCode();
        }
    }
}
