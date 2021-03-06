﻿using LiquidTrouse.Core.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error : System.Web.UI.Page
{
    protected i18nHelper i18n;

    protected void Page_Load(object sender, EventArgs e)
    {
        var ex = Server.GetLastError();
        if (ex != null)
        {
            Global.Log.Error(ex.Message, ex);
        }
        i18n = new i18nHelper();
    }
}