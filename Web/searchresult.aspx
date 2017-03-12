<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="searchresult.aspx.cs" Inherits="searchresult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/pager.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var searchServiceUrl = rootUrl + "/service/ajaxsearchservice.aspx";
        var mode;
        var gKeyword = "<%=Keyword%>";
        var gSortBy = "<%=SortBy%>";
        var gSortDirection = "<%=SortDirection%>";
        var gTag = "<%=TagDisplayName%>";
        var gPageIndex = <%=PageIndex%>;
        var gPageSize = <%=PageSize%>;
        var offset = "<%=WebUtility.GetCurrentUserUTCOffset().ToString()%>";

        $(document).ready(function () {

            var pagerOption = {
                PreviousPage: "<%=i18n.GetMessage("m20")%>",
                NextPage: "<%=i18n.GetMessage("m21")%>",
                Info: "<%=i18n.GetMessage("m22")%>"
            };
            InitPager(pagerOption);

            if (gKeyword != null && gKeyword != "") {
                $("#hint").html("<%=i18n.GetMessage("m30")%>：「" + gKeyword + "」");
                mode = "keyword";
            }
            if (gTag != null && gTag != "") {
                $("#hint").html("<%=i18n.GetMessage("m29")%>：「" + gTag + "」");
                mode = "tag";
            }
            WriteArticles();
        });

        function WriteArticles() {
            var param = "cmd=" + mode
                + "&keyword=" + LambdaEncode(gKeyword)
                + "&sortby=" + gSortBy
                + "&sortdirection=" + gSortDirection
                + "&pageindex=" + gPageIndex
                + "&pagesize=" + gPageSize
                + "&tag=" + LambdaEncode(gTag);
            $.getJSON(searchServiceUrl, param, Callback)
        }
        function Callback(data) {
            var results = new Array();
            var totalCount = 0;
            if (data.Success) {
                $.each(data.Data.Results, function (i, n) {
                    results.push(n);
                });
                totalCount = data.Data.TotalCount;
                gPageIndex = data.Data.PageIndex;
                gPageSize = data.Data.PageSize;
            }

            if (totalCount <= 0) {
                $("#articles").html(GetNoDataStyle("<%=i18n.GetMessage("m31")%>"));
                return;
            }

            var html = "";
            $.each(results, function (i, n) {
                html += RenderResult(rootUrl, n, offset, "<%=i18n.GetMessage("m24")%>");
            });
            $("#articles").html(html);

            WritePager(gPageIndex, gPageSize, totalCount);
        }
    </script>
    <header class="intro-header">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="site-heading">
                        <h1 class="tagline"><%=i18n.GetMessage("m1")%></h1>
                        <div class="subheading tagline"><%=i18n.GetMessage("m11")%></div>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <div class="container">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">
                    <small id="hint"></small>
                </h1>
            </div>
        </div>
        <div id="articles"></div>
        <div class="row text-center">
            <ul id="pager" class="pager"></ul>
        </div>
    </div>
</asp:Content>
