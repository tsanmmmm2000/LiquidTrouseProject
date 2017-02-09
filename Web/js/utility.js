function LambdaEncode(x) {
    var tx = encodeURIComponent(x);
    tx = tx.replace(/\'/g, "%27");
    tx = tx.replace(/\)/g, "%29");
    tx = tx.replace(/~/g, "%7E");
    tx = tx.replace(/!/g, "%21");
    tx = tx.replace(/\(/g, "%28");
    return tx;
}
function LambdaDecode(x) {
    x = x.replace(/%27/g, "'");
    x = x.replace(/%29/g, ")");
    x = x.replace(/%7E/g, "~");
    x = x.replace(/%21/g, "!");
    x = x.replace(/%28/g, "(");
    x = decodeURIComponent(x);
    return x;
}
function SimpleHtmlEncode(value) {
    return $('<div/>').text(value).html();
}
function SimpleHtmlDecode(value) {
    return $('<div/>').html(value).text();
}
function RenderResult(rootUrl, articelObj, offset) {
    var html = "";
    html += "<div class=\"row\">";
    html += "<div class=\"col-md-4\">";
    var articleUrl = rootUrl + "/article/" + articelObj.UrlTitle + "/" + articelObj.ArticleId;
    html += "<a href=\"" + articleUrl + "\">";
    var coverImageUrl = (articelObj.CoverImageUrl != null && articelObj.CoverImageUrl != "") ? articelObj.CoverImageUrl : rootUrl + "/image/banner_3.jpg";
    html += "<img class=\"img-responsive\" src=\"" + coverImageUrl + "\">";
    html += "</a>";
    html += "</div>";
    html += "<div class=\"col-md-7\">";
    html += "<h3>";
    html += "<a href=\"" + articleUrl + "\">";
    html += articelObj.Title;
    html += "</a>"
    html += "</h3>";
    html += "<p>";
    html += "<span class=\"glyphicon glyphicon-time\"></span>&nbsp;";
    html += FormatDateTime(ConvertJsonDateTimeToDateTime(articelObj.CreationDatetime), offset);
    html += "&nbsp;&nbsp;<span class=\"glyphicon glyphicon-user\"></span>&nbsp;"
    html += articelObj.UserId;
    html += "</p>";
    html += "<p>";
    var content = StripHtml(articelObj.Content);
    html += content.substring(0, parseInt(200, 10));
    html += "&nbsp;...";
    html += "</p>";
    html += "<a class=\"btn btn-primary\" href=\"" + articleUrl + "\">";
    html += "閱讀更多&nbsp;";
    html += "<span class=\"glyphicon glyphicon-chevron-right\"></span>";
    html += "</a>";
    html += "</div>";
    html += "</div>"
    html += "<hr>";
    return html;
}
function ConvertJsonDateTimeToDateTime(date) {
    var jsonDateTime = date.replace("\/Date(", "").replace(")\/", "");
    var d = new Date(parseInt(jsonDateTime, 10));
    return d;
}
function ConvertToISO8601WithUserTimeZone(date, offset) {
    if (date == "") {
        return "";
    }

    if (offset == null) {
        offset = "00:00:00";
    }
    var hms = offset.split(':');
    var h = parseInt(hms[0], 10);
    var m = parseInt(hms[1], 10);
    var s = parseInt(hms[2], 10);

    if (h > 0) {
        date.setHours(date.getHours() - h, date.getMinutes() - m, date.getSeconds() - s);
    } else {
        date.setHours(date.getHours() + Math.abs(h), date.getMinutes() + Math.abs(m), date.getSeconds() + Math.abs(s));
    }
    var yyyy = date.getFullYear();
    var MM = date.getMonth() + 1;
    var dd = date.getDate();
    var HH = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    MM = AddLeadingZero(MM, 2);
    dd = AddLeadingZero(dd, 2);
    HH = AddLeadingZero(HH, 2);
    mm = AddLeadingZero(mm, 2);
    ss = AddLeadingZero(ss, 2);
    var rs = yyyy.toString() + MM.toString() + dd.toString() + HH.toString() + mm.toString() + ss.toString();

    return rs;
}
function ConvertToISO8601(date) {
    var yyyy = date.getUTCFullYear();
    var MM = date.getUTCMonth() + 1;
    var dd = date.getUTCDate();
    var HH = date.getUTCHours();
    var mm = date.getUTCMinutes();
    var ss = date.getUTCSeconds();
    MM = AddLeadingZero(MM, 2);
    dd = AddLeadingZero(dd, 2);
    HH = AddLeadingZero(HH, 2);
    mm = AddLeadingZero(mm, 2);
    ss = AddLeadingZero(ss, 2);
    var s = yyyy.toString() + MM.toString() + dd.toString() + HH.toString() + mm.toString() + ss.toString();
    return s;
}
function FormatDateTime(date, offset) {
    var s = "";
    var hms = offset.split(":");
    var displayDate = new Date(date);
    displayDate.setUTCHours(
        displayDate.getHours() + parseInt(hms[0], 10),
        displayDate.getMinutes() + parseInt(hms[1], 10),
        displayDate.getSeconds() + parseInt(hms[2], 10)
    );
    s = displayDate.getUTCFullYear() + "/" + AddLeadingZero((displayDate.getUTCMonth() + 1), 2) + "/" + AddLeadingZero(displayDate.getUTCDate(), 2) + " " + AddLeadingZero(displayDate.getUTCHours(), 2) + ":" + AddLeadingZero(displayDate.getUTCMinutes(), 2);
    return s;
}
function StripHtml(input) {
    var output = '';
    if (typeof (input) == 'string') {
        var output = input.replace(/(<([^>]+)>)/ig, "");
    }
    return output;
}
function AddLeadingZero(num, padLength) {
    var s = "";
    if (num.toString().length < padLength) {
        for (var i = 1; i < padLength; i++) {
            s += "0";
        }
    }
    s += num.toString();
    return s;
}
function AlertMessage(message) {
	$(".alert-danger").show();
	$(".alert-danger").html(message);
}