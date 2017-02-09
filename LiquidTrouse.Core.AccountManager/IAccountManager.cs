using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager
{
    public interface IAccountManager
    {
        UserInfo[] GetAll();
        UserInfo[] GetAdmins();
        UserInfo[] GetGuests();
        UserInfo GetByUserId(string userId);
        UserInfo GetByLoginId(string loginId);
        bool CheckPassword(string loginId, string password);

        string ConfigFilePath { get; set; }
    }
}
