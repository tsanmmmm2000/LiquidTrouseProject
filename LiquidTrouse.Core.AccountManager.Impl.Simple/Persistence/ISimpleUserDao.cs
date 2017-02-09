using LiquidTrouse.Core.AccountManager.Impl.Simple.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager.Impl.Simple.Persistence
{
    public interface ISimpleUserDao
    {
        IList GetAll();
        SimpleUser GetByUserId(string userId);
        SimpleUser GetByLoginId(string loginId);
        void Create(SimpleUser user);
        void Update(SimpleUser user);
        void Delete(string userId);
    }
}
