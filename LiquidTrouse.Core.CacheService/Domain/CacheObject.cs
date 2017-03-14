using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LiquidTrouse.Core.CacheService.Domain
{
    [Serializable]
    [DataContract]
    public class CacheObject
    {
        private object _data;
        private Type _dataType = typeof(object);
        private DateTime _absoluteExpiration = CacheUtility.NoAbsoluteExpiration;
        private TimeSpan _slidingExpiration = CacheUtility.DefaultSlidingExpiration;

        [DataMember]
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        [DataMember]
        public Type DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        [DataMember]
        public DateTime AbsoluteExpiration
        {
            get { return _absoluteExpiration; }
            set { _absoluteExpiration = value; }
        }

        [DataMember]
        public TimeSpan SlidingExpiration
        {
            get { return _slidingExpiration; }
            set { _slidingExpiration = value; }
        }
    }
}
