using System;
using System.Runtime.Serialization;

namespace LiquidTrouse.Core.Blog.Service.DTO
{
    [Serializable]
    [DataContract]
    public class ArticleInfo
    {
        private int _articleId = -1;
        private string _userId = string.Empty;
        private DateTime _creationDatetime = DateTime.UtcNow;
        private DateTime _lastModifiedDatetime = DateTime.UtcNow;
        private string _title = string.Empty;
        private string _content = string.Empty;
        private string _urlTitle = string.Empty;
        private string _coverImageUrl = string.Empty;

        [DataMember]
        public int ArticleId
        {
            get { return _articleId; }
            set { _articleId = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        [DataMember]
        public DateTime CreationDatetime
        {
            get { return _creationDatetime; }
            set { _creationDatetime = value; }
        }

        [DataMember]
        public DateTime LastModifiedDatetime
        {
            get { return _lastModifiedDatetime; }
            set { _lastModifiedDatetime = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [DataMember]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        [DataMember]
        public string UrlTitle
        {
            get { return _urlTitle; }
            set { _urlTitle = value; }
        }

        [DataMember]
        public string CoverImageUrl
        {
            get { return _coverImageUrl; }
            set { _coverImageUrl = value; }
        }
    }
}
