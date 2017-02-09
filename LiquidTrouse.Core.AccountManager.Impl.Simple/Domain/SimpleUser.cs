using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager.Impl.Simple.Domain
{
    public class SimpleUser
    {
        private string _userId = string.Empty;
        private string _loginId = string.Empty;
        private string _password = string.Empty;
        private string _displayName = string.Empty;

        public SimpleUser() { }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string LoginId
        {
            get { return _loginId; }
            set { _loginId = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
