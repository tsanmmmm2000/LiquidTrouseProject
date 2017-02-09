<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="readarticle.aspx.cs" Inherits="readarticle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <script type="text/javascript">
        var offset = "<%=WebUtility.GetCurrentUserUTCOffset().ToString()%>";

        $(document).ready(function () {
            WriteResult();
            WriteTags();		
            $(".btn-tag").click(function () {
		        var tagDisplayName = $(this).text();
		        window.location.href = rootUrl + "/search/tag/" + LambdaEncode($.trim(tagDisplayName));
		    });
        });
        function WriteResult() {
            var articleInfo = $.parseJSON($("#articleInfo").val());
            var hitCount = $("#hitCount").val();
            var title = articleInfo.Title;
            var content = articleInfo.Content;
            var coverImageUrl = articleInfo.CoverImageUrl;
            var meta = "<span class=\"glyphicon glyphicon-time\"></span>&nbsp;";
            meta += FormatDateTime(ConvertJsonDateTimeToDateTime(articleInfo.CreationDatetime), offset);
            meta += "&nbsp;&nbsp;<span class=\"glyphicon glyphicon-user\"></span>&nbsp;"
            meta += articleInfo.UserId;
            meta += "&nbsp;&nbsp;<span class=\"glyphicon glyphicon-eye-open\"></span>&nbsp;"
            meta += hitCount;
            $("#title").append(title);
            $("#article-content").append(content);
            $(".meta").append(meta);
            $("#cover-image-url").css("background-image", "url('" + coverImageUrl + "')");
        }
        function WriteTags() {
            var tagInfos = $.parseJSON($("#tagInfos").val());
            var html = "";
            $.each(tagInfos, function (i, n) {
				var displayName = n.DisplayName;
				html += "<button type=\"button\" class=\"btn btn-warning btn-sm btn-tag\" style=\"margin-right: 5px; margin-top: 5px;\">";
				html += displayName;
				html += "</button>";
            });
            $("#tag-section").html(html);
        }
    </script>
    <header id="cover-image-url" class="intro-header">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="site-heading">
                        <h1 class="tagline" id="title"></h1>
                        <div class="subheading tagline meta"></div>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <div class="container">
        <div class="row">
            <div class="col-lg-12">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-lg-8 col-lg-offset-2">
                <div id="article-content"></div>
                <div id="tag-section"></div>
            </div>
        </div>
    </div>
</asp:Content>
