using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LiquidTrouse.Core.AccountManager.DTO
{
    [Serializable]
    [DataContract]
    public class UserInfo
    {
        private string _userId = string.Empty;
        private string _loginId = string.Empty;
        private string _displayName = string.Empty;
        private string _timeZone = string.Empty;

        public UserInfo() { }

        [DataMember]
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        [DataMember]
        public string LoginId
        {
            get { return _loginId; }
            set { _loginId = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        [DataMember]
        public string TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
        }
    }
}
