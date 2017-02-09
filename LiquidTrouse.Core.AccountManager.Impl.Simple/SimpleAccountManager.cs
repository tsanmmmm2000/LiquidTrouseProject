using LiquidTrouse.Core.AccountManager.Base;
using LiquidTrouse.Core.AccountManager.DTO;
using LiquidTrouse.Core.AccountManager.Impl.Simple.Converter;
using LiquidTrouse.Core.AccountManager.Impl.Simple.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace LiquidTrouse.Core.AccountManager.Impl.Simple
{
    public class SimpleAccountManager : BaseAccountManager
    {
        private SimpleUserConverter _converter = new SimpleUserConverter();

        private ISimpleUserDao _simpleUserDao;
        public ISimpleUserDao SimpleUserDao
        {
            set { _simpleUserDao = value; }
        }

        private string _salt;
        public string Salt
        {
            set { _salt = value; }
        }

        public override string ConfigFilePath { get; set; }

        public override UserInfo[] GetAll()
        {
            var users = _simpleUserDao.GetAll();
            return _converter.ToDataTransferObject(users);
        }
        public override UserInfo GetByUserId(string userId)
        {
            var user = _simpleUserDao.GetByUserId(userId);
            return _converter.ToDataTransferObject(user);
        }
        public override UserInfo GetByLoginId(string loginId)
        {
            var user = _simpleUserDao.GetByLoginId(loginId);
            return _converter.ToDataTransferObject(user);
        }
        public override bool CheckPassword(string loginId, string password)
        {
            var user = _simpleUserDao.GetByLoginId(loginId);

            if (user != null)
            {
                var temp = String.Concat(password, _salt);
                var hash = FormsAuthentication.HashPasswordForStoringInConfigFile(temp, "SHA1");

                if (user != null && user.Password == hash)
                {
                    return true;
                }
                return false;
            }
            throw new Exception("The user does not exist.");
        }
    }
}
