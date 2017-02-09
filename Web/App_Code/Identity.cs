using LiquidTrouse.Core.AccountManager;
using LiquidTrouse.Core.AccountManager.DTO;
using System.Security.Principal;

public class Identity : IIdentity
{
    private UserInfo _user;
    private string _name;

    private Identity() { }
    public Identity(string ticketName, string userId)
    {
        var accountManager = AccountUtility.AccountManagerRepository.GetAccountManager();
        _user = accountManager.GetByUserId(userId);
        _name = ticketName;
    }

    public UserInfo User
    {
        get { return _user; }
    }

    public string AuthenticationType
    {
        get { return "Authentication"; }
    }

    public bool IsAuthenticated
    {
        get { return true; }
    }

    public string Name
    {
        get { return _name; }
    }
}