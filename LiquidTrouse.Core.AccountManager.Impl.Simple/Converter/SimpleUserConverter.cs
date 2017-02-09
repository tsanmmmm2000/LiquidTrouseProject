using LiquidTrouse.Core.AccountManager.Base;
using LiquidTrouse.Core.AccountManager.DTO;
using LiquidTrouse.Core.AccountManager.Impl.Simple.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager.Impl.Simple.Converter
{
    public class SimpleUserConverter : BaseUserConverter<SimpleUser>
    {
        public override UserInfo[] ToDataTransferObject(IList users)
        {
            var userInfoList = new List<UserInfo>();
            if (users != null && users.Count > 0)
            {
                foreach (SimpleUser user in users)
                {
                    var userInfo = new UserInfo();
                    userInfo.UserId = user.UserId;
                    userInfo.LoginId = user.LoginId;
                    userInfo.DisplayName = user.DisplayName;
                    userInfoList.Add(userInfo);
                }
            }
            return userInfoList.ToArray();
        }

        public override UserInfo ToDataTransferObject(SimpleUser user)
        {
            return ToDataTransferObject(new List<SimpleUser>() { user })[0];
        }
    }
}
