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
    </style>
    <header id="cover-image-url" class="intro-header">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="site-heading">
                        <h1 class="tagline">關於我</h1>
                    </div>
                </div>
            </div>
        </div>
    </header>

    <div class="container">

        <hr class="featurette-divider">

        <div class="featurette text-center">
            <img class="featurette-image img-circle img-responsive pull-right" src="<%=Request.ApplicationPath %>/image/logo_3.jpg">
            <h2><span class="glyphicon glyphicon-tint" aria-hidden="true"></span>&nbsp;Meaning</h2>
            <p>&nbsp;</p>            
            <p>Trouse，它，既是 Trance Music，也是 House Music</p>
            <p>Liquid Spirit，它，是文化的根源，是孕育萬物的能量，更有著無限的包容</p>
            <p>它，帶來快樂也帶走憂傷，是對生命態度的執著</p>
            <p>將兩者放在一起...它，Liquid Trouse 便是象徵著美好的體驗</p>
        </div>

        <hr class="featurette-divider">

        <div class="featurette text-center">
            <img class="featurette-image img-circle img-responsive pull-left" src="<%=Request.ApplicationPath %>/image/logo_1.jpg">
            <h2><span class="glyphicon glyphicon-education" aria-hidden="true"></span>&nbsp;Vision</h2>
            <p>&nbsp;</p>
            <p>在這裡，我們分享 Trouse Music，集結同好並彼此交流。</p>
            <p>在這裡，我們希望能讓更多台灣人認識 Trance & House 文化，大家一起學習一起成長！</p>
            <p>在這裡，我們秉持 Liquid Spirit，找回聽音樂的初衷，感受音樂就如同感受水的溫柔。</p>
            <p>音樂，它無時無刻環繞於我們四周，帶給我們快樂及正面的能量，使我們面對更多挑戰！</p>
            <p>歡迎來到 Liquid Trouse 電音誌，與我們一起釋放心靈感受音樂的饗宴吧！</p>
        </div>

        <hr class="featurette-divider">

        <div class="featurette text-center" style="margin-bottom:80px;">
            <img class="featurette-image img-circle img-responsive pull-right" src="<%=Request.ApplicationPath %>/image/logo_2.jpg">
            <h2><span class="glyphicon glyphicon-send" aria-hidden="true"></span>&nbsp;Contact</h2>
            <p>&nbsp;</p>
            <p>YouTube: <a target="_blank" href="https://www.youtube.com/channel/UCx2Q4Fps54yqTuJqEXrBLpA">Liquid Trouse</a></p>
            <p>Instagram: <a target="_blank" href="https://www.instagram.com/liquidtrouse/">Liquid Trouse</a></p>
            <p>Facebook: <a target="_blank" href="https://www.facebook.com/LiquidTrouse">LiquidTrouse 電音誌</a></p>
            <p>Email: <a href="mailto:LiquidTrouse@gmail.com">LiquidTrouse@gmail.com</a></p>
        </div>
    </div>
</asp:Content>
