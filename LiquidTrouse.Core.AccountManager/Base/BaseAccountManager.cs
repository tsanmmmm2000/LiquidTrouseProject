using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace LiquidTrouse.Core.AccountManager.Base
{
    public abstract class BaseAccountManager : IAccountManager
    {
        private const string BuiltInUserConfigFile = "BuiltInUserConfig.xml";
        private const string UsersNode = "users";
        private const string UserNode = "user";
        private const string AdminAttribute = "admin";
        private const string GuestAttribute = "guest";

        public abstract string ConfigFilePath { get; set; }

        public abstract UserInfo[] GetAll();
        public abstract UserInfo GetByUserId(string userId);
        public abstract UserInfo GetByLoginId(string loginId);
        public abstract bool CheckPassword(string loginId, string password);

        public UserInfo[] GetAdmins()
        {
            return GetBuiltInUsers(AdminAttribute);
        }
        public UserInfo[] GetGuests()
        {
            return GetBuiltInUsers(GuestAttribute);
        }

        private XmlDocument LoadXml()
        {
            var xml = new XmlDocument();
            try
            {
                var filePath = Path.Combine(ConfigFilePath, BuiltInUserConfigFile);
                var xmlCache = new XmlCache(filePath);
                xml = xmlCache.Read();
            }
            catch (Exception ex)
            {
                Utility.ErrorLog(ex.Message, ex);
            }
            return xml;
        }
        private UserInfo[] GetBuiltInUsers(string type)
        {
            var builtInUserList = new List<UserInfo>();
            var xml = LoadXml();
            var builtInUserNodes = xml.SelectNodes(UsersNode + "/" + UserNode + "[@type='" + type + "']");
            if (builtInUserNodes != null)
            {
                foreach (XmlNode builtInUserNode in builtInUserNodes)
                {
                    var builtInUserId = builtInUserNode.Attributes["userid"].Value;
                    var builtInUserInfo = GetByUserId(builtInUserId);
                    builtInUserList.Add(builtInUserInfo);
                }
            }
            return builtInUserList.ToArray();
        }
    }
}
