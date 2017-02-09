using System;
using System.Runtime.Serialization;

namespace LiquidTrouse.Core.Blog.Service.DTO
{
    [Serializable]
    [DataContract]
    public class TagInfo
    {
        private int _tagId = -1;
        private string _displayName = string.Empty;
        private int _usedCount = 1;
        private DateTime _lastUsedDatetime = DateTime.UtcNow;

        [DataMember]
        public int TagId
        {
            get { return _tagId; }
            set { _tagId = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        [DataMember]
        public int UsedCount
        {
            get { return _usedCount; }
            set { _usedCount = value; }
        }

        [DataMember]
        public DateTime LastUsedDatetime
        {
            get { return _lastUsedDatetime; }
            set { _lastUsedDatetime = value; }
        }
    }
}
