using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class searchresult : System.Web.UI.Page
{
    protected string Keyword;
    protected string SortBy;
    protected string SortDirection;
    protected string TagDisplayName;
    protected int PageIndex;
    protected int PageSize;

    protected void Page_Load(object sender, EventArgs e)
    {
        Keyword = HttpUtility.HtmlDecode(WebUtility.GetRoutingStringParameter(this.Page, "keyword", string.Empty));
        SortBy = WebUtility.GetRoutingStringParameter(this.Page, "sortby", "creationdatetime");
        SortDirection = WebUtility.GetRoutingStringParameter(this.Page, "sortdirection", "desc");
        TagDisplayName =  HttpUtility.HtmlDecode(WebUtility.GetRoutingStringParameter(this.Page, "tag", string.Empty));
        PageIndex = WebUtility.GetRoutingIntegerParameter(this.Page, "pageindex", 0);
        PageSize = WebUtility.GetRoutingIntegerParameter(this.Page, "pagesize", 10);
    }
}