using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _default : System.Web.UI.MasterPage
{
    protected bool IsAdmin = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        var currentUser = WebUtility.GetCurrentUser();
        IsAdmin = WebUtility.IsAdminUser(currentUser);
    }
}
