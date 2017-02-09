using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core.AccountManager;
using LiquidTrouse.Core.AccountManager.DTO;
using Newtonsoft.Json;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Globalization;
using System.Web.UI;

public class WebUtility
{
    private static IApplicationContext _applicationContext;
    public static IApplicationContext ApplicationContext
    {
        get
        {
            var currentCtx = ContextRegistry.GetContext();
            if (_applicationContext == null)
            {
                _applicationContext = currentCtx;
            }
            else
            {
                if (!Object.ReferenceEquals(_applicationContext, currentCtx))
                {
                    _applicationContext = currentCtx;
                }
            }
            return _applicationContext;
        }
        set { _applicationContext = value; }
    }
    
    private static RepositoryFactory _repository;
    public static RepositoryFactory Repository
    {
        get
        {
            _repository = new RepositoryFactory(ApplicationContext);
            return _repository;
        }
    }

    static WebUtility()
    {
        var ctx = ContextRegistry.GetContext();
        if (ApplicationContext == null)
        {
            ApplicationContext = ctx;
        }
        else
        {
            if (!Object.ReferenceEquals(ApplicationContext, ctx))
            {
                ApplicationContext = ctx;
            }
        }
    }

    public static int GetIntegerParameter(string paraName)
    {
        return GetIntegerParameter(paraName, 0);
    }
    public static int GetIntegerParameter(string paraName, int defaultValue)
    {
        var paraValue = defaultValue;
        var s = GetStringParameter(paraName);
        if (!String.IsNullOrEmpty(s))
        {
            paraValue = Convert.ToInt32(s);
        }
        return paraValue;
    }
    public static int GetRoutingIntegerParameter(Page page, string paraName)
    {
        return GetRoutingIntegerParameter(page, paraName, 0);
    }
    public static int GetRoutingIntegerParameter(Page page, string paraName, int defaultValue)
    {
        var paraValue = defaultValue;
        var s = GetRoutingStringParameter(page, paraName);
        if (!String.IsNullOrEmpty(s))
        {
            paraValue = Convert.ToInt32(s);
        }
        return paraValue;
    }
    public static string RewrittenContent(string html)
    {
        html = Regex.Replace(html, @"<script[\d\D]*?>[\d\D]*?</script>", string.Empty);
        html = Regex.Replace(html, @"<[^>]*>", string.Empty);
        return html;
    }
    public static string GetAjaxCommand()
    {
        return GetStringParameter("cmd").ToLower();
    }
    public static string GetStringParameter(string paraName)
    {
        return GetStringParameter(paraName, string.Empty);
    }
    public static string GetStringParameter(string paraName, string defaultValue)
    {
        if (HttpContext.Current.Request.QueryString[paraName] != null && HttpContext.Current.Request.QueryString[paraName] != "null")
        {
            defaultValue = HttpContext.Current.Request.QueryString[paraName].Trim();
        }
        else
        {
            if (HttpContext.Current.Request.Form[paraName] != null)
            {
                defaultValue = HttpContext.Current.Request.Form[paraName].Trim();
            }
        }
        return ToAntiXSSHtmlEncode(defaultValue);
    }
    public static string GetRoutingStringParameter(Page page, string paraName)
    {
        return GetRoutingStringParameter(page, paraName, string.Empty);
    }
    public static string GetRoutingStringParameter(Page page, string paraName, string defaultValue)
    {
        if (page.RouteData.Values[paraName] != null && page.RouteData.Values[paraName] != "null")
        {
            defaultValue = ToAntiXSSHtmlEncode(page.RouteData.Values[paraName].ToString().Trim());
        }
        else
        {
            defaultValue = GetStringParameter(paraName, defaultValue);
        }
        return defaultValue;
    }
    public static string SerializeToJson(object objectToSerialize)
    {
        return JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
    }
    public static object DeserializeAjaxResult(string jsonString, Type type)
    {
        return JsonConvert.DeserializeObject(jsonString, type);
    }
    public static void WriteAjaxResult(AjaxResult result)
    {
        HttpContext.Current.Response.Expires = 0;
        HttpContext.Current.Response.CacheControl = "no-cache";
        HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
    }
    public static void WriteAjaxResult(bool success, string message, object data)
    {
        WriteAjaxResult(GenAjaxResult(success, message, data));
    }
    public static void WriteTranslatedAjaxError(Exception ex)
    {
        var errorMessage = ex.Message;
        Global.Log.Error(errorMessage, ex);
        WriteAjaxResult(GenAjaxResult(false, errorMessage, ex));
    }
    public static bool IsAdminUser(UserInfo userInfo)
    {
        return AccountUtility.IsAdminUser(userInfo);
    }
    public static UserInfo GetCurrentUser()
    {
        var identity = GetCurrentIdentity();
        return (identity != null) ? identity.User : AccountUtility.AssignGuestUser();
    }
	public static TimeSpan GetCurrentUserUTCOffset()
    {
        return GetUserUTCOffset(GetCurrentUser());
    }
    public static TimeSpan GetUserUTCOffset(UserInfo userInfo)
    {
        var defaultTimeZoneInfo = TimeZoneInfo.Local;

        var t = userInfo.TimeZone;
        if (String.IsNullOrWhiteSpace(t))
        {
            t = defaultTimeZoneInfo.BaseUtcOffset + "@" + defaultTimeZoneInfo.Id;
        }

        TimeSpan ts;
        try
        {
            var tz = t.Split('@');
            var h = tz[0].Split(':')[0];
            var m = tz[0].Split(':')[1];
            ts = new TimeSpan(Convert.ToInt32(h), Convert.ToInt32(m), 0);
        }
        catch
        {
            ts = new TimeSpan(Convert.ToInt32(defaultTimeZoneInfo.BaseUtcOffset.Hours), Convert.ToInt32(defaultTimeZoneInfo.BaseUtcOffset.Minutes), 0);
        }
        return ts;
    }
    public static void SignOut()
    {
        FormsAuthentication.SignOut();
    }
    public static string ConvertToISO8601SimpleString(DateTime dateTime)
    {
        return dateTime.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo);
    }
    public static DateTime ParseISO8601SimpleString(string inputString)
    {
        var year = int.Parse(inputString.Substring(0, 4));
        var month = int.Parse(inputString.Substring(4, 2));
        var date = int.Parse(inputString.Substring(6, 2));
        var hour = int.Parse(inputString.Substring(8, 2));
        var minute = int.Parse(inputString.Substring(10, 2));
        var second = int.Parse(inputString.Substring(12, 2));
        return new DateTime(year, month, date, hour, minute, second, DateTimeKind.Utc);
    }


    private static string ToAntiXSSHtmlEncode(string s)
    {
        return Microsoft.Security.Application.Encoder.HtmlEncode(s);
    }
    private static AjaxResult GenAjaxResult(bool success, string message, object data)
    {
        var result = new AjaxResult();
        result.Success = success;
        result.Message = message;
        result.Data = data;
        return result;
    }
    private static Identity GetCurrentIdentity()
    {
        var principal = HttpContext.Current.User as Principal;
        if (principal != null)
        {
            var identity = principal.Identity as Identity;
            if (identity != null)
            {
                return identity;
            }
        }
        return null;
    }

}
