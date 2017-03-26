using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace LiquidTrouse.Core.Globalization
{
    public class i18nHelper
    {
        public string GetMessage(string messageKey)
        {
            var ctx = Utility.ApplicationContext;
            return ctx.GetMessage(messageKey, GetCultureInfo());
        }

        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            var cookie = new HttpCookie(CultureConstants.Language, cultureInfo.Name)
            {
                Path = FormsAuthentication.FormsCookiePath,
                Expires = new DateTime(9999, 12, 31)
            };
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        public CultureInfo GetCultureInfo()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(CultureConstants.Language);
            if (cookie == null)
            {
                switch (CultureInfo.CurrentCulture.ToString().ToLower())
                {
                    case CultureConstants.ZhTw:
                        return new CultureInfo(CultureConstants.ZhTw);
                    case CultureConstants.En:
                        return new CultureInfo(CultureConstants.En);
                    default:
                        return new CultureInfo(CultureConstants.ZhTw);
                }
            }
            else
            {
                return new CultureInfo(cookie.Value);
            }
        }
    }
}
