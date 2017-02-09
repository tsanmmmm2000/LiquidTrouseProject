using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager
{
    public class AccountUtility
    {
        private static AccountManagerRepositoryFactory _accountManagerRepository;
        public static AccountManagerRepositoryFactory AccountManagerRepository
        {
            get
            {
                _accountManagerRepository = new AccountManagerRepositoryFactory(Utility.ApplicationContext);
                return _accountManagerRepository;
            }
        }

        private static IAccountManager _accountManager
        {
            get { return AccountManagerRepository.GetAccountManager(); }
        }

        public static UserInfo AssignGuestUser()
        {
            return _accountManager.GetGuests()[0];
        }
        public static bool IsAdminUser(UserInfo userInfo)
        {
            var adminUserInfos = _accountManager.GetAdmins();
            if (adminUserInfos != null && adminUserInfos.Length > 0)
            {
                foreach (var adminUserInfo in adminUserInfos)
                {
                    if (adminUserInfo.UserId.Equals(userInfo.UserId, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
