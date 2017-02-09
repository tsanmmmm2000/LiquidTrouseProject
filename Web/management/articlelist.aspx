<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="articlelist.aspx.cs" Inherits="management_articlelist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/pager.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var articleServiceUrl = rootUrl + "/service/ajaxarticleservice.aspx";
        var langPager = {
            prePage: "上一頁",
            nextPage: "下一頁",
            pagerInfo: "總共有{0}篇文章，共{1}頁"
        };
        var gPageIndex = <%=PageIndex%>;
        var gPageSize = <%=PageSize%>;
        $(document).ready(function () {
            SetPagerText(langPager.prePage, langPager.nextPage, langPager.pagerInfo);
            WriteUI();

            $("#btn-create").click(function() {
                window.location.href = rootUrl + "/management/create";
            });

            $("#btn-back").click(function() {
                window.location.href = rootUrl + "/default";
            });

            $("#btn-logout").click(function() {
                window.location.href = rootUrl + "/management/logout";
            });
        });

        function UpdatePagerParamAndCallShowResult(pageIndex, pageSize) {
            gPageIndex = parseInt(pageIndex, 10);
            gPageSize = parseInt(pageSize, 10);
            WriteUI()
        }

        function WriteUI() {
            $("#articles").html("");
            $("#pager").html("");
            ShowResult(gPageIndex, gPageSize);
        }
        function ShowResult(pageIndex, pageSize) {
            var param = "cmd=batchget"
                + "&pageindex=" + pageIndex
                + "&pagesize=" + pageSize;
            $.getJSON(articleServiceUrl, param, GetResult)
        }
        function GetResult(data) {
            arrResult = new Array();
            var pageIndexFromResult = 0;
            if (data.Success) {
                $.each(data.Data.PageOfResults, function (i, n) {
                    arrResult.push(n);
                });
                totalCount = data.Data.TotalCount;
                pageIndexFromResult = data.Data.PageNumber;
            } else {
                totalCount = 0;
            }

            if (data.Data.TotalCount <= 0) {
                $("#articles").html(GetNoDataStyle());
                return;
            }
            WriteResult();

            var pageCount;
            if (gPageIndex == null) {
                gPageIndex = 0;
            }
            if (gPageSize == null) {
                gPageSize = 10;
            }
            if ((totalCount % gPageSize) == 0) {
                pageCount = totalCount / gPageSize;
            } else {
                pageCount = parseInt((totalCount / gPageSize).toString(), 10) + 1;
            }
            CreatePagerUsingPagerJS((pageIndexFromResult != gPageIndex ? pageIndexFromResult : gPageIndex), gPageSize, pageCount, totalCount);
        }
        function WriteResult() {
            $("#articles").html("");
            var html = "";
            $.each(arrResult, function (i, n) {
                html += "<tr>";
                html += "<td>"
                html += "<a href=\"" + rootUrl + "/article/" + n.UrlTitle + "/" + n.ArticleId + "\" target=\"_blank\">"
                html += n.Title;
                html += "</a>";
                html += "</td>"
                html += "<td align=\"right\">";
                html += "<button id=\"edit-" + n.ArticleId + "\" type=\"button\" class=\"btn btn-warning btn-edit\">";
                html += "編輯";
                html += "</button>";
                html += "<button id=\"delete-" + n.ArticleId + "\" type=\"button\" class=\"btn btn-danger btn-delete\">";
                html += "刪除";
                html += "</button>";
                html += "</td>";
                html += "</tr>"
            });
            $("#articles").html(html);

            $(".btn-edit").click(function(){
                var articleId = this.id.split('-')[1];
                window.location.href = rootUrl + "/management/edit/" + articleId;
            });
            
            $(".btn-delete").click(function() {
                if(confirm("確定刪除？")) {
                    var articleId = this.id.split('-')[1];
                    var param = "cmd=delete" +
                        "&articleid=" + articleId;
                    $.post(articleServiceUrl, param, function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success) {
                            alert("刪除成功");
                            $("#articles").empty();
                            WriteUI();
                        } else {
                            Alert(result.Message);
                        }
                    });
                }
            });
        }
		function GetNoDataStyle() {
			return "<h4>查無文章</h4>"
		}		
    </script>
    <style type="text/css">
        body {
	        padding: 50px 100px;
        }
        .table>tbody>tr>td {
            vertical-align: middle !important;
        }
    </style>
    <button type="button" class="btn btn-primary" id="btn-create">建立文章</button>
    <button type="button" class="btn btn-success" id="btn-back">返回網站</button>
    <button type="button" class="btn btn-default" id="btn-logout" style="float:right; margin-right:14px;">登出</button>
    <table class="table table-hover">
        <thead>
          <tr>
            <th></th>
            <th></th>
          </tr>
        </thead>
        <tbody id="articles"></tbody>
    </table>
    <ul id="articles" class="list-group"></ul>
    <ul id="pager" class="pager"></ul>
</asp:Content>
