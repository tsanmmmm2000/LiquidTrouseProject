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
        private const string Language = "LiquidTrouse_Language";

        public string GetMessage(string messageKey)
        {
            var ctx = Utility.ApplicationContext;
            return ctx.GetMessage(messageKey, GetCultureInfo());
        }

        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            var cookie = new HttpCookie(Language, cultureInfo.Name)
            {
                Path = FormsAuthentication.FormsCookiePath,
                Expires = new DateTime(9999, 12, 31)
            };
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        public CultureInfo GetCultureInfo()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(Language);
            if (cookie == null)
            {
                switch (CultureInfo.CurrentCulture.ToString().ToLower())
                {
                    case "zh-TW":
                        return new CultureInfo("zh-TW");
                    case "en":
                        return new CultureInfo("en");
                    default:
                        return new CultureInfo("zh-TW");
                }
            }
            else
            {
                return new CultureInfo(cookie.Value);
            }
        }
    }
}
