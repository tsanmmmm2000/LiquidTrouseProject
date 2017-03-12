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
                PreviousPage: "<%=i18n.GetMessage("m20")%>",
                NextPage: "<%=i18n.GetMessage("m21")%>",
                Info: "<%=i18n.GetMessage("m22")%>"
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
                $("#articles").html(GetNoDataStyle("<%=i18n.GetMessage("m31")%>"));
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
                html += "<%=i18n.GetMessage("m38")%>";
                html += "</button>";
                html += "<button id=\"delete-" + n.ArticleId + "\" type=\"button\" class=\"btn btn-danger btn-delete\">";
                html += "<%=i18n.GetMessage("m39")%>";
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
                if(confirm("<%=i18n.GetMessage("m49")%>")) {
                    var articleId = this.id.split('-')[1];
                    var param = "cmd=delete" +
                        "&articleid=" + articleId;
                    $.post(articleServiceUrl, param, function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success) {
                            alert("<%=i18n.GetMessage("m50")%>");
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
    <button type="button" class="btn btn-primary" id="btn-create"><%=i18n.GetMessage("m36")%></button>
    <button type="button" class="btn btn-success" id="btn-back"><%=i18n.GetMessage("m37")%></button>
    <button type="button" class="btn btn-default" id="btn-logout" style="float:right; margin-right:14px;"><%=i18n.GetMessage("m35")%></button>
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
