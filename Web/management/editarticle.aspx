<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="editarticle.aspx.cs" Inherits="management_editarticle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%=Request.ApplicationPath%>/bootstrap/css/bootstrap-datetimepicker.min.css<%="" + Global.Quid%>" />
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/ckeditor/ckeditor.js<%="" + Global.Quid%>""></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/bootstrap/js/moment.min.js<%=Global.Quid %>"></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/bootstrap/js/bootstrap-datetimepicker.min.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var articleServiceUrl = rootUrl + "/service/ajaxarticleservice.aspx";
        var tagServiceUrl = rootUrl + "/service/ajaxtagservice.aspx";
        var articleId = <%=ArticleId%>;
        var offset = "<%=WebUtility.GetCurrentUserUTCOffset().ToString()%>";

        $(document).ready(function () {
            CKEDITOR.replace("editor", { height: "350px", width: "90%" });
            WriteResult();
            WriteTags();

            $("#btn-update").click(function () {
                var editorValue = CKEDITOR.instances.editor.getData();
                var title = $("#article-title").val();
                if (title == null || title == "") {
                    alert("標題為必填");
                    return;
                }
                var urlTitle = $("#url-title").val();
                if (urlTitle == null || urlTitle == "") {
                    alert("網址標題為必填");
                    return;
                }
                var param = "cmd=update" +
                    "&articleid=" + articleId +
                    "&title=" + LambdaEncode(title) +
                    "&urltitle=" + urlTitle +
                    "&coverimageurl=" + $("#cover-image-url").val() +
                    "&content=" + LambdaEncode(editorValue) +
                    "&tags=" + LambdaEncode($("#tags").val());

                var creationDatetime = ConvertToISO8601WithUserTimeZone(new Date($("#creation-date").val()), offset);
                if (!isNaN(creationDatetime)) {
                    param += "&creationdatetime=" + creationDatetime;
                }

                $.post(articleServiceUrl, param, function (data) {
                    var result = $.parseJSON(data);
                    var redirecturl = rootUrl + "/management/list";
                    if (result.Success) {
                        alert("更新成功");
                        window.location.href = redirecturl;
                    } else {
                        AlertMessage(result.Message);
                    }
                });
            });

            $("#btn-cancel").click(function () {
                if (CheckDirty()) {
                    if (confirm("內容已被變更，仍確定離開？")) {
                        window.location.href = rootUrl + "/management/list";
                    }
                } else {
                    window.location.href = rootUrl + "/management/list";
                }
            });
        });
        function CheckDirty() {

            var articleInfo = $.parseJSON($("#articleInfo").val());

            var title = articleInfo.Title;
            var newTitle = $("#article-title").val();
            if (title != newTitle) {
                return true;
            }

            var urlTitle = articleInfo.UrlTitle;
            var newUrlTitle = $("#url-title").val();
            if (urlTitle != newUrlTitle) {
                return true;
            }

            var content = articleInfo.Content;
            var newContent = CKEDITOR.instances.editor.getData();
            if (content.trim() != newContent.trim()) {
                return true;
            }

            var coverImageUrl = articleInfo.CoverImageUrl;
            var newCoverImageUrl = $("#cover-image-url").val();
            if (coverImageUrl != newCoverImageUrl) {
                return true;
            }

            var tags = "";
            var tagInfos = $.parseJSON($("#tagInfos").val());
            if (tagInfos.length > 0) {
                $.each(tagInfos, function (i, n) {
                    tags += n.DisplayName + ", ";
                });
            }
            var newTags = $("#tags").val();
            if (tags != newTags) {
                return true;
            }

            return false;
        }
        function WriteTags() {
            var param = "cmd=getall";
            $.post(tagServiceUrl, param, function (data) {
                var result = $.parseJSON(data);
                if (result.Success) {
                    var html = "";
                    $.each(result.Data, function(i, n){
                        var displayName = n.DisplayName;
                        html += "<li><span id=\"" + n.TagId + "\" class=\"tag-text\">";
                        html += displayName;
                        html += "</span></li>";
                        html += "<li><span>,</span></li>";
                    });
                    $("#tag-options").append(html);

                    $(".tag-text").click(function () {
                        var selectedTagTextInLowerCase = $.trim($(this).text()).toLowerCase();
                        var inputValue = $.trim($("#tags").val());
                        var inputValueInLowerCase = inputValue.toLowerCase();
                        var inputTagsArray = inputValueInLowerCase.split(",");
                        if ($(this).hasClass("selected-tag")) {
                            $(".tag-text").each(function () {
                                if ($.trim($(this).text()).toLowerCase() == selectedTagTextInLowerCase) {
                                    $(this).removeClass("selected-tag");
                                }
                            });
                            var remainStr = "";
                            for (var ia = 0; ia < inputTagsArray.length; ia++) {
                                var tagText = $.trim(inputTagsArray[ia]);
                                if (tagText != selectedTagTextInLowerCase && tagText != "") {
                                    remainStr += tagText + ", ";
                                }
                            }
                            $("#tags").val(remainStr);
                        } else {
                            $(this).addClass("selected-tag");
                            $(".tag-text").each(function () {
                                if ($.trim($(this).text()).toLowerCase() == selectedTagTextInLowerCase) {
                                    $(this).addClass("selected-tag");
                                }
                            });
                            var match = false;
                            for (var ib = 0; ib < inputTagsArray.length; ib++) {
                                if ($.trim(inputTagsArray[ib]) == selectedTagTextInLowerCase) {
                                    match = true;
                                    break;
                                }
                            }

                            if (!match) {
                                if (inputValueInLowerCase.match(/\s*,+\s*$/)) {
                                    $("#tags").val(inputValueInLowerCase + " " + $(this).text() + ", ");
                                } else if (inputValueInLowerCase.replace(/^\s*,*|\s*,*$/, "") == "") {
                                    $("#tags").val($(this).text() + ", ");
                                } else {
                                    $("#tags").val(inputValueInLowerCase + ", " + $(this).text() + ", ");
                                }
                            }
                        }
                    });

                    $(".tag-text").hover(
                    function () {
                        $(this).addClass("hover-select-tag");
                    }, function () {
                        $(this).removeClass("hover-select-tag");
                    });

                    var tagInfos = $.parseJSON($("#tagInfos").val());
                    $.each(tagInfos, function (i, n) {
                        $("#" + n.TagId).click();
                    });

                } else {
                    AlertMessage(result.Message);
                }
            });
        }
        function WriteResult() {
            var articleInfo = $.parseJSON($("#articleInfo").val());
            var title = articleInfo.Title;
            var urlTitle = articleInfo.UrlTitle;
            var content = articleInfo.Content;
            var coverImageUrl = articleInfo.CoverImageUrl;
            var creationDatetime = FormatDateTime(ConvertJsonDateTimeToDateTime(articleInfo.CreationDatetime), offset);
            $("#article-title").val(title);
            $("#cover-image-url").val(coverImageUrl);
            $("#url-title").val(urlTitle);
            $("#datetimepicker").datetimepicker({
                date: creationDatetime
            });
            CKEDITOR.instances.editor.setData(content);
        }
    </script>
    <style type="text/css">
        .tag-options li {
            color: #357EC7;
            cursor: pointer;
            display: inline-block;
            padding: 0 1px;
            text-decoration: none;
            line-height: 1.5em;
            list-style-image: none;
            list-style-position: outside;
            list-style-type: none;
        }
        .selected-tag {
            background: #848484;
            color: white;
        }
        .hover-select-tag {
            background: #2E2E2E;
            color: white;
        }
        .tag-text {
            padding-left: 3px;
            padding-right: 2px;
            white-space: nowrap;
        }
        body {
	        padding: 10px 30px;
        }
    </style>
    <table>
        <tr>
            <td>
				<button type="button" class="btn btn-primary" id="btn-update">更新</button>
                <button type="button" class="btn btn-default" id="btn-cancel">取消</button>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td valign="top" width="70%">
                <textarea name="editor" id="editor"></textarea>   
            </td>
            <td valign="top" width="30%">
				<div class="input-group">
					<span class="input-group-addon">標題</span>
					<input type="text" class="form-control" id="article-title" placeholder="example">
				</div>
                <br/>
				<div class="input-group">
					<span class="input-group-addon">首圖連結</span>
					<input type="text" class="form-control" id="cover-image-url" placeholder="http://example/example.jpg">
				</div>					  
                <br/>
				<div class="input-group">
					<span class="input-group-addon">http://yourdomain/liquidtrouse/article/</span>
					<input type="text" class="form-control" id="url-title" placeholder="網址標題">
					<span class="input-group-addon">/</span>					
				</div>
				<br/>
                <div class="input-group date" id="datetimepicker">
                    <input type="text" class="form-control" id="creation-date" placeholder="建立時間">
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
				<br/>
                <textarea class="form-control" rows="5" id="tags" placeholder="標籤"></textarea>
                <div class="tag-options" id="tag-options"></div>
            </td>
        </tr>
    </table>
</asp:Content>
