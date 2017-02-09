using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.DTO
{
    [Serializable]
    [DataContract]
    public class HitInfo
    {
        private int _hitId = -1;
        private int _resourceId = -1;
        private string _ipAddress = string.Empty;
        private DateTime _creationDatetime;

        [DataMember]
        public int HitId
        {
            get { return _hitId; }
            set { _hitId = value; }
        }

        [DataMember]
        public int ResourceId
        {
            get { return _resourceId; }
            set { _resourceId = value; }
        }

        [DataMember]
        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        [DataMember]
        public DateTime CreationDatetime
        {
            get { return _creationDatetime; }
            set { _creationDatetime = value; }
        }
    }
}
