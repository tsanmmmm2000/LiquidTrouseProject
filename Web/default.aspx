<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/pager.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var articleServiceUrl = rootUrl + "/service/ajaxarticleservice.aspx";
        var sortElement = {
            creationdatetime: "<%=i18n.GetMessage("m25")%>",
            lastmodifieddatetime: "<%=i18n.GetMessage("m26")%>",
            hit: "<%=i18n.GetMessage("m27")%>",
            title: "<%=i18n.GetMessage("m28")%>"
        }
        var gPageIndex = <%=PageIndex%>;
        var gPageSize = <%=PageSize%>;
        var gSortBy = "<%=SortBy%>";
        var gSortDirection = "<%=SortDirection%>";
        var offset = "<%=WebUtility.GetCurrentUserUTCOffset().ToString()%>";

        $(document).ready(function () {
            var pagerOption = {
                PreviousPage: "<%=i18n.GetMessage("m20")%>",
                NextPage: "<%=i18n.GetMessage("m21")%>",
                Info: "<%=i18n.GetMessage("m22")%>"
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
                    <small><%=i18n.GetMessage("m23")%></small>
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

