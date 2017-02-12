<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/pager.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var articleServiceUrl = rootUrl + "/service/ajaxarticleservice.aspx";
        var sortElement = {
            creationdatetime: "依建立時間",
            lastmodifieddatetime: "依修改時間",
            hit: "依熱門度",
            title: "依標題"
        }
        var gPageIndex = <%=PageIndex%>;
        var gPageSize = <%=PageSize%>;
        var gSortBy = "<%=SortBy%>";
        var gSortDirection = "<%=SortDirection%>";
        var offset = "<%=WebUtility.GetCurrentUserUTCOffset().ToString()%>";

        $(document).ready(function () {
            var pagerOption = {
                PreviousPage: "上一頁",
                NextPage: "下一頁",
                Info: "總共有 {0} 篇文章，共 {1} 頁"
            };
            InitPager(pagerOption);
            WriteArticles();
            WriteSortingDropdown();
        });

        function WriteArticles() {
            var param = "cmd=batchget"
                + "&pageindex=" + gPageIndex
                + "&pagesize=" + gPageSize 
                + "&sortby=" + gSortBy
                + "&sortdirection=" + gSortDirection;
            $.getJSON(articleServiceUrl, param, Callback)
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
		function WriteSortingDropdown() {
		    $(".dropdown-menu").empty();
		    $.each(sortElement, function(i, n) {
		        if (i != gSortBy) {
		            var item = "<li><a href=\"javascript:void(0);\" class=\"sort-by\" id=\"" + i + "\">" + n + "</a></li>";
		            $(".dropdown-menu").append(item);
		        }
		    });
		    $(".dropdown-toggle").html(sortElement[gSortBy] + "&nbsp;<span class=\"caret\"></span>");
		    $(".sort-by").click(function() {
		        gSortBy = this.id;
		        WriteArticles();
		        WriteSortingDropdown();
		    });
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
                    <small>文章列表</small>
                    <div class="dropdown" style="float:right;">
                      <button class="btn btn-success dropdown-toggle" type="button" data-toggle="dropdown"></button>
                      <ul class="dropdown-menu dropdown-menu-right"></ul>
                    </div>
                </h1>

            </div>
        </div>
        <div id="articles"></div>
        <div class="row text-center">
            <ul id="pager" class="pager"></ul>
        </div>
    </div>
</asp:Content>

