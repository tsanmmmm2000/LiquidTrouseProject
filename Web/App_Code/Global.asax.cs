using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;

public partial class Global : System.Web.HttpApplication
{
    public static string Quid = "?liquidtrouse";
    public static log4net.ILog Log;

    protected static void RegisterRoutes(RouteCollection routes)
    {
        routes.Ignore("{resource}.axd/{*pathInfo}");
        foreach (var routeRule in RouteHandler.RouteRules)
        {
            try
            {
                routes.MapPageRoute(
                    routeRule.RouteName,
                    routeRule.RouteUrl,
                    routeRule.PhysicalFile
                );
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
    }


    protected void Application_Start(object sender, EventArgs e)
    {
        // 在應用程式啟動時執行的程式碼
        log4net.Config.XmlConfigurator.Configure();
        Log = log4net.LogManager.GetLogger("LQTS");
        RegisterRoutes(RouteTable.Routes);
    }

    protected void Application_End(object sender, EventArgs e)
    {
        //  在應用程式關閉時執行的程式碼

    }

    protected void Application_Error(object sender, EventArgs e)
    {
        // 在發生未處理的錯誤時執行的程式碼

    }

    protected void Session_Start(object sender, EventArgs e)
    {
        // 在新的工作階段啟動時執行的程式碼

    }

    protected void Session_End(object sender, EventArgs e)
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        var cookieName = FormsAuthentication.FormsCookieName;
        var authCookie = Context.Request.Cookies[cookieName];
        if (null == authCookie)
        {
            return;
        }

        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        if (null == authTicket)
        {
            return;
        }
        if (authTicket.UserData == string.Empty)
        {
            return;
        }

        try
        {
            var userData = authTicket.UserData;
            var userId = userData.Split('^')[0];
            var principle = new Principal(authTicket.Name, userId);
            Context.User = principle;
        }
        catch(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }
    } 
}