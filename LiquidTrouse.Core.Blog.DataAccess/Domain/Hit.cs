using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess.Domain
{
    public enum HitType
    {
        Article,
        Tag
    }

    public class Hit
    {
        private int _hitId = -1;
        private int _resourceId = -1;
        private string _ipAddress = string.Empty;
        private HitType _hitType = HitType.Article;
        private DateTime _creationDatetime;

        public Hit() { }

        public int HitId
        {
            get { return _hitId; }
            set { _hitId = value; }
        }

        public int ResourceId
        {
            get { return _resourceId; }
            set { _resourceId = value; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public HitType HitType
        {
            get { return _hitType; }
            set { _hitType = value; }
        }

        public DateTime CreationDatetime
        {
            get { return _creationDatetime; }
            set { _creationDatetime = value; }
        }
    }
}
