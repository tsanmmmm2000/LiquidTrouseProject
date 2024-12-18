﻿<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true" CodeFile="createarticle.aspx.cs" Inherits="management_createarticle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%=Request.ApplicationPath%>/bootstrap/css/bootstrap-datetimepicker.min.css<%="" + Global.Quid%>" />
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/ckeditor/ckeditor.js<%="" + Global.Quid%>""></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/bootstrap/js/moment.min.js<%=Global.Quid %>"></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/bootstrap/js/bootstrap-datetimepicker.min.js<%=Global.Quid %>"></script>
    <script type="text/javascript">
        var rootUrl = "<%=Request.ApplicationPath%>";
        var articleServiceUrl = rootUrl + "/service/ajaxarticleservice.aspx";
        var tagServiceUrl = rootUrl + "/service/ajaxtagservice.aspx";
        var offset = "<%=WebUtility.GetCurrentUserUTCOffset().ToString()%>";

        $(document).ready(function () {
            CKEDITOR.replace("editor", { height: "350px", width: "90%" });
            GetTags();

            $("#btn-submit").click(function () {
                var editorValue = CKEDITOR.instances.editor.getData();
                var title = $("#article-title").val();
                if (title == null || title == "") {
                    alert("<%=i18n.GetMessage("m51")%>");
                    return;
                }

                var urlTitle = $("#url-title").val();
                if (urlTitle == null || urlTitle == "") {
                    alert("<%=i18n.GetMessage("m52")%>");
                    return;
                }
                var param = "cmd=create" +
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
                        alert("<%=i18n.GetMessage("m53")%>");
                        window.location.href = redirecturl;
                    } else {
                        AlertMessage(result);
                    }
                });
            });

            $("#btn-cancel").click(function () {
                window.location.href = rootUrl + "/management/list";
            });

            $("#datetimepicker").datetimepicker();
        });
        function GetTags() {
            var param = "cmd=getall";
            $.post(tagServiceUrl, param, function (data) {
                var result = $.parseJSON(data);
                if (result.Success) {
                    var html = "";
                    $.each(result.Data, function(i, n){
                        var displayName = n.DisplayName;
                        html += "<li><span class=\"tag-text\">";
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
                } else {
                    AlertMessage(result);
                }
            });
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
    <div>
        <table>
            <tr>
                <td>
				    <button type="button" class="btn btn-primary" id="btn-submit"><%=i18n.GetMessage("m45")%></button>
                    <button type="button" class="btn btn-success" id="btn-draft"><%=i18n.GetMessage("m44")%></button>
                    <button type="button" class="btn btn-default" id="btn-cancel"><%=i18n.GetMessage("m41")%></button>            					
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
					    <span class="input-group-addon"><%=i18n.GetMessage("m28")%></span>
					    <input type="text" class="form-control" id="article-title" placeholder="example">
				    </div>
                    <br/>
				    <div class="input-group">
					    <span class="input-group-addon"><%=i18n.GetMessage("m42")%></span>
					    <input type="text" class="form-control" id="cover-image-url" placeholder="http://example/example.jpg">
				    </div>					  
                    <br/>
				    <div class="input-group">
					    <span class="input-group-addon">http://yourdomain/liquidtrouse/article/</span>
					    <input type="text" class="form-control" id="url-title" placeholder="<%=i18n.GetMessage("m43")%>">
					    <span class="input-group-addon">/</span>					
				    </div>
				    <br/>
                    <div class="input-group date" id="datetimepicker">
                        <input type="text" class="form-control" id="creation-date" placeholder="<%=i18n.GetMessage("m25")%>">
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
				    <br/>
                    <textarea class="form-control" rows="5" id="tags" placeholder="<%=i18n.GetMessage("m17")%>"></textarea>
                    <div class="tag-options" id="tag-options"></div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
