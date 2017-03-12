using LiquidTrouse.Core.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _default : System.Web.UI.Page
{
    protected int PageIndex;
    protected int PageSize;
    protected string SortBy;
    protected string SortDirection;
    protected i18nHelper i18n;

    protected void Page_Load(object sender, EventArgs e)
    {
        PageIndex = WebUtility.GetRoutingIntegerParameter(this.Page, "pageindex", 0);
        PageSize = WebUtility.GetRoutingIntegerParameter(this.Page, "pagesize", 10);
        SortBy = WebUtility.GetRoutingStringParameter(this.Page, "sortby", "creationdatetime");
        SortDirection = WebUtility.GetRoutingStringParameter(this.Page, "sortdirection", "desc");
        i18n = new i18nHelper();
    }
}