using LiquidTrouse.Core.AccountManager;
using LiquidTrouse.Core.Globalization;
using System;
using System.Web;
using System.Web.Security;

public partial class management_login : System.Web.UI.Page
{
    private IAccountManager _accountManager;

    protected i18nHelper i18n;

    protected void Page_Load(object sender, EventArgs e)
    {
        _accountManager = AccountUtility.AccountManagerRepository.GetAccountManager();
        i18n = new i18nHelper();
    }

    protected void LoginButtonClick(object sender, EventArgs e)
    {
        RecoverFailureHint();
        if (ValidateControl())
        {
            SignIn();
        }
    }

    private void SignIn()
    {
        if (Authenticate())
        {
            var loginId = txtUserName.Value.Trim();
            SetFormsAuthTicketAndCookies(loginId);
            var query = WebUtility.GetRoutingStringParameter(this.Page, "ReturnUrl");
            var url = (!String.IsNullOrEmpty(query) && !IsSelf(query))
                ? query
                : Request.ApplicationPath + "/management/list";
            Response.Redirect(url, false);
        }
        else
        {
            SetFailureHint(i18n.GetMessage("m56"));
        }
    }

    private bool IsSelf(string url)
    {
        return (FormsAuthentication.LoginUrl.Equals(url + ".aspx", StringComparison.InvariantCultureIgnoreCase))
            ? true
            : false;
    }

    private void SetFormsAuthTicketAndCookies(string loginId)
    {
        var userInfo = _accountManager.GetByLoginId(loginId);
        var userId = userInfo.UserId;
        var guid = Guid.NewGuid().ToString();
        var authTicketUserData = String.Format("{0}^{1}", userId, guid);
        var ticket = new FormsAuthenticationTicket(1, loginId, DateTime.UtcNow, DateTime.Now.AddMinutes(30), false, authTicketUserData);
        var hashTicket = FormsAuthentication.Encrypt(ticket);
        var userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
        Response.Cookies.Add(userCookie);
    }

    private bool Authenticate()
    {
        var authenticate = false;
        var userName = txtUserName.Value.Trim();
        var password = txtPassword.Value;

        try
        {
            if (_accountManager.CheckPassword(userName, password))
            {
                authenticate = true;
            }
            return authenticate;
        }
        catch (Exception ex)
        {
            Global.Log.Error(ex.Message, ex);
            SetFailureHint(i18n.GetMessage("m57"));
            return authenticate;
        }
    }

    private bool ValidateControl()
    {
        bool validated = true;

        if (txtUserName.Value.Trim() == string.Empty)
        {
            userNameRequired.Attributes["style"] = "color: Red; visibility: visible;";
            validated = false;
        }
        else
        {
            userNameRequired.Attributes["style"] = "color: Red; visibility: hidden;";
        }

        if (txtPassword.Value.Trim() == string.Empty)
        {
            passwordRequired.Attributes["style"] = "color: Red; visibility: visible;";
            validated = false;
        }
        else
        {
            passwordRequired.Attributes["style"] = "color: Red; visibility: hidden;";
        }

        return validated;
    }

    private void SetFailureHint(string hint)
    {
        txtFailure.Attributes["title"] = string.Empty;
        txtFailure.InnerHtml = Microsoft.Security.Application.Encoder.HtmlEncode(hint);
        txtFailure.Attributes["style"] = "color: Red; visibility: visible;";
    }

    private void RecoverFailureHint()
    {
        txtFailure.Attributes["title"] = string.Empty;
        txtFailure.InnerHtml = string.Empty;
        txtFailure.Attributes["style"] = "color: Red; visibility: hidden;";
    }
}