using LiquidTrouse.Core.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class management_createarticle : System.Web.UI.Page
{
    protected i18nHelper i18n;

    protected void Page_Load(object sender, EventArgs e)
    {
        i18n = new i18nHelper();
    }
}