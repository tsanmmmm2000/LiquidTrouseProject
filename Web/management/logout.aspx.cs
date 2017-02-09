using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class management_logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtility.SignOut();
        var query = WebUtility.GetRoutingStringParameter(this.Page, "ReturnUrl");
        //var url = (!String.IsNullOrEmpty(query))
        //    ? String.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, HttpUtility.UrlEncode(query))
        //    : FormsAuthentication.DefaultUrl;
        var url = (!String.IsNullOrEmpty(query)) ? query : Request.ApplicationPath + "/management/list";
        Response.Redirect(url, false);
    }
}