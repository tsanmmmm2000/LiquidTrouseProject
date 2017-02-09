<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="management_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Liquid Trouse 電音誌</title>
    <link rel="icon" href="../image/logo.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="../image/logo.ico" type="image/x-icon" />
    <link rel="bookmark" href="image/logo.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="<%=Request.ApplicationPath%>/bootstrap/css/bootstrap.min.css<%="" + Global.Quid%>" />
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/jquery-1.11.2.js<%="" + Global.Quid%>""></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/bootstrap/js/bootstrap.min.js<%=Global.Quid %>"></script>
    <style type="text/css">
        .logo-img {
	        width: 95px;
	        height: 95px;
	        margin: 0 auto 10px;
	        display: block;
	        -moz-border-radius: 50%;
	        -webkit-border-radius: 50%;
	        border-radius: 50%;
        }
    </style>
</head>
<body>
    <div class="container" style="margin-top:50px">
		<div class="row">
			<div class="col-sm-6 col-md-4 col-md-offset-4">
				<div class="panel panel-default">
					<div class="panel-heading"></div>
					<div class="panel-body">
						<form id="form1" runat="server">
							<fieldset>
								<div class="row">
									<div class="center-block">
										<img class="logo-img" src="../image/logo.png" />
									</div>
								</div>
                                <br/>
								<div class="row">
									<div class="col-sm-12 col-md-10  col-md-offset-1 ">
										<div class="form-group">
											<div class="input-group">
												<span class="input-group-addon">
													<i class="glyphicon glyphicon-user"></i>
												</span> 
												<input class="form-control" type="text" name="txtUserName" id="txtUserName" placeholder="使用者名稱" runat="server" autofocus />
											</div>
                                            <span id="userNameRequired" style="color: Red; visibility: hidden;" runat="server">*</span>
										</div>
										<div class="form-group">
											<div class="input-group">
												<span class="input-group-addon">
													<i class="glyphicon glyphicon-lock"></i>
												</span>
												<input class="form-control" type="password" name="txtPassword" id="txtPassword" placeholder="密碼" runat="server" />
											</div>
                                            <span id="passwordRequired" style="color: Red; visibility: hidden;" runat="server">*</span>
										</div>
                                        <div class="form-group">
                                            <asp:Button ID="LoginButton" runat="server" Text="登入" OnClick="LoginButtonClick" CssClass="btn btn-lg btn-primary btn-block"/>
										</div>
                                        <div align="center" id="txtFailure" style="color: Red; visibility: hidden;" runat="server" />
									</div>
								</div>
							</fieldset>
						</form>
					</div>
                    <div class="panel-footer">
                        <a href="<%=Request.ApplicationPath %>/default">返回網站</a>
                    </div>
                </div>
			</div>
		</div>
	</div>
</body>
</html>
