using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public sealed class AjaxResult
{

    private bool success = false;
    private string message = string.Empty;
    private Object data = null;

    public AjaxResult(){}

    public bool Success
    {
        get { return success; }
        set { success = value; }
    }

    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    public Object Data
    {
        get { return data; }
        set { data = value; }
    }
}
