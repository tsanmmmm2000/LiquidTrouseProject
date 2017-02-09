using System;
using System.Collections;

namespace LiquidTrouse.Core.Blog.DataAccess.Domain
{
    public enum ArticleStatus
    {
        Active,
        InActive
    }

    public class Article
    {
        private int _articleId = -1;
        private string _userId = string.Empty;
        private DateTime _creationDatetime = DateTime.UtcNow;
        private DateTime _lastModifiedDatetime = DateTime.UtcNow;
        private string _title = string.Empty;
        private string _content = string.Empty;
        private string _urlTitle = string.Empty;
        private string _coverImageUrl = string.Empty;
        private ArticleStatus _status = ArticleStatus.Active;
        private IList _tags = new ArrayList();

        public Article(){ }

        public int ArticleId
        {
            get { return _articleId; }
            set { _articleId = value; }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public DateTime CreationDatetime
        {
            get { return _creationDatetime; }
            set { _creationDatetime = value; }
        }

        public DateTime LastModifiedDatetime
        {
            get { return _lastModifiedDatetime; }
            set { _lastModifiedDatetime = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string UrlTitle
        {
            get { return _urlTitle; }
            set { _urlTitle = value; }
        }

        public string CoverImageUrl
        {
            get { return _coverImageUrl; }
            set { _coverImageUrl = value; }
        }

        public ArticleStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public IList Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    }
}
