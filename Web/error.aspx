<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Liquid Trouse 電音誌</title>
    <link rel="icon" href="../image/logo.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="../image/logo.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="<%=Request.ApplicationPath%>/bootstrap/css/bootstrap.min.css<%="" + Global.Quid%>" />
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/jquery-1.11.2.js<%="" + Global.Quid%>""></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/bootstrap/js/bootstrap.min.js<%=Global.Quid %>"></script>
</head>
<body>
    <div>
        <form id="form1" runat="server">
            <div class="container">
                <div class="jumbotron" style="margin-top: 50px;">
                  <h2>抱歉，發生未預期的錯誤</h2>
                  <br/>
                  <a class="btn btn-primary btn-lg" href="default" role="button">按此離開</a>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
