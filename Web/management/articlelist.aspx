<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="articlelist.aspx.cs" Inherits="management_articlelist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/pager.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var articleServiceUrl = rootUrl + "/service/ajaxarticleservice.aspx";
        var gPageIndex = <%=PageIndex%>;
        var gPageSize = <%=PageSize%>;
        $(document).ready(function () {
            var pagerOption = {
                PreviousPage: "上一頁",
                NextPage: "下一頁",
                Info: "總共有 {0} 篇文章，共 {1} 頁"
            };
            InitPager(pagerOption);
            WriteArticles();

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

        function WriteArticles() {
            var param = "cmd=batchget"
                + "&pageindex=" + gPageIndex
                + "&pagesize=" + gPageSize;
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

            WritePager(gPageIndex, gPageSize, totalCount);

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
                            WriteArticles();
                        } else {
                            Alert(result.Message);
                        }
                    });
                }
            });
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
