using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager.Base
{
    public abstract class BaseUserConverter<T>
    {
        public abstract UserInfo[] ToDataTransferObject(IList users);
        public abstract UserInfo ToDataTransferObject(T user);
    }
}
