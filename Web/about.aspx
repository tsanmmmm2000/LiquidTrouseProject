<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="about" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%=Request.ApplicationPath%>/bootstrap/css/one-page-wonder.css<%="" + Global.Quid%>" />
   <script type="text/javascript">
       var rootUrl = "<%=Request.ApplicationPath%>";
       var coverImageUrl = rootUrl + "/image/banner_2.jpg";
       $(document).ready(function () {
           $("#cover-image-url").css("background-image", "url('" + coverImageUrl + "')");
       });
    </script>
    <style type="text/css">
        .img-circle {
            width: 300px;
            height: 300px;
        }
        h2>.glyphicon {
            top: 3px !important;
        }
    </style>
    <header id="cover-image-url" class="intro-header">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="site-heading">
                        <h1 class="tagline"><%=i18n.GetMessage("m16") %></h1>
                    </div>
                </div>
            </div>
        </div>
    </header>

    <div class="container">

        <hr class="featurette-divider">

        <div class="featurette text-center">
            <img class="featurette-image img-circle img-responsive pull-right" src="<%=Request.ApplicationPath %>/image/logo_3.jpg">
            <h2><span class="glyphicon glyphicon-leaf" aria-hidden="true"></span>&nbsp;<%=i18n.GetMessage("m12") %></h2>
            <p>&nbsp;</p>            
            <p><%=i18n.GetMessage("m2") %></p>
            <p><%=i18n.GetMessage("m3") %></p>
            <p><%=i18n.GetMessage("m4") %></p>
            <p><%=i18n.GetMessage("m5") %></p>
        </div>

        <hr class="featurette-divider">

        <div class="featurette text-center">
            <img class="featurette-image img-circle img-responsive pull-left" src="<%=Request.ApplicationPath %>/image/logo_1.jpg">
            <h2><span class="glyphicon glyphicon-education" aria-hidden="true"></span>&nbsp;<%=i18n.GetMessage("m13") %></h2>
            <p>&nbsp;</p>
            <p><%=i18n.GetMessage("m6") %></p>
            <p><%=i18n.GetMessage("m7") %></p>
            <p><%=i18n.GetMessage("m8") %></p>
            <p><%=i18n.GetMessage("m9") %></p>
            <p><%=i18n.GetMessage("m10") %></p>
        </div>

        <hr class="featurette-divider">

        <div class="featurette text-center" style="margin-bottom:80px;">
            <img class="featurette-image img-circle img-responsive pull-right" src="<%=Request.ApplicationPath %>/image/logo_2.jpg">
            <h2><span class="glyphicon glyphicon-send" aria-hidden="true"></span>&nbsp;<%=i18n.GetMessage("m14") %></h2>
            <p>&nbsp;</p>
            <p>Instagram: <a target="_blank" href="https://www.instagram.com/liquidtrouse/">LiquidTrouse</a></p>
            <p>Facebook: <a target="_blank" href="https://www.facebook.com/LiquidTrouse"><%=i18n.GetMessage("m1") %></a></p>            
            <p>YouTube: <a target="_blank" href="https://www.youtube.com/channel/UCx2Q4Fps54yqTuJqEXrBLpA">Liquid Trouse Channel</a></p>
            <p>Email: <a href="mailto:LiquidTrouse@gmail.com">LiquidTrouse@gmail.com</a></p>
        </div>
    </div>
</asp:Content>
