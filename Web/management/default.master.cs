using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class management_default : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var currentUser = WebUtility.GetCurrentUser();
        var isAdmin = WebUtility.IsAdminUser(currentUser);
        if (!isAdmin)
        {
            Response.Redirect(Request.ApplicationPath + "/management/login", false);
        }
    }
}
