using System;
using System.Collections;

namespace LiquidTrouse.Core.Blog.DataAccess.Domain
{
    public class Tag
    {
        private int _tagId = -1;
        private string _displayName = string.Empty;
        private int _usedCount = 0;
        private DateTime _lastUsedDatetime = DateTime.UtcNow;
        private IList _articles = new ArrayList();

        public Tag() { }

        public int TagId
        {
            get { return _tagId; }
            set { _tagId = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public int UsedCount
        {
            get { return _usedCount; }
            set { _usedCount = value; }
        }

        public DateTime LastUsedDatetime
        {
            get { return _lastUsedDatetime; }
            set { _lastUsedDatetime = value; }
        }

        public IList Articles
        {
            get { return _articles; }
            set { _articles = value; }
        }
    }
}
