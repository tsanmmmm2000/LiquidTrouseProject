using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.AccountManager
{
    public class AccountManagerRepositoryFactory
    {
        public AccountManagerRepositoryFactory() { }

        public AccountManagerRepositoryFactory(IApplicationContext context)
        {
            Utility.ApplicationContext = context;
        }

        public IAccountManager GetAccountManager()
        {
            return Utility.ApplicationContext["AccountManager"] as IAccountManager;
        }
    }
}
