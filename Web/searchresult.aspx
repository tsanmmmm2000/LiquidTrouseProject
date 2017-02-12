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
                PreviousPage: "上一頁",
                NextPage: "下一頁",
                Info: "總共有 {0} 篇文章，共 {1} 頁"
            };
            InitPager(pagerOption);

            if (gKeyword != null && gKeyword != "") {
                $("#hint").html("搜尋關鍵字：「" + gKeyword + "」");
                mode = "keyword";
            }
            if (gTag != null && gTag != "") {
                $("#hint").html("搜尋標籤：「" + gTag + "」");
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
                $("#articles").html(GetNoDataStyle());
                return;
            }

            var html = "";
            $.each(results, function (i, n) {
                html += RenderResult(rootUrl, n, offset);
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
                        <h1 class="tagline">Liquid Trouse 電音誌</h1>
                        <div class="subheading tagline">幾個對電音及派對文化有興趣的年輕人所開的網誌，專門分享 Trance & House 音樂及相關資訊</div>
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
