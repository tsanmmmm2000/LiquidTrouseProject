var prePageForPagerJS = "pre";
var nextPageForPagerJS = "next";
var pagerInfoForPagerJS = "{0} articles,{1}pages total";

function SetPagerText(prePageText, nextPageText, pagerInfoText) {
    SetPrePageText(prePageText);
    SetNextPageText(nextPageText);
    SetPagerInfoText(pagerInfoText);
}

function InspectPagerText(text) {
    if (typeof text !== "undefined")
        if (text != null)
            if (text != "")
                return true;
    return false;
}

function SetPrePageText(text) {
    if (InspectPagerText(text))
        prePageForPagerJS = text;
}

function SetNextPageText(text) {
    if (InspectPagerText(text))
        nextPageForPagerJS = text;
}

function SetPagerInfoText(text) {
    if (InspectPagerText(text))
        pagerInfoForPagerJS = text;
}

function CreatePagerUsingPagerJS(pageIndex, pageSize, pageCount, totalCount) {
    var pagerHtml = "";
    var min = CalculateCurrentMin(pageIndex, pageCount);
    var max = CalculateCurrentMax(pageIndex, pageCount, min);
    var baseLink = "UpdatePagerParamAndCallShowResult({0},{1});";
    var tempLink = "";

    if (pageIndex > 0) {
        tempLink = StringFormat(baseLink, pageIndex - 1, pageSize);
        pagerHtml += "<li class=\"previous\">";
        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"" + tempLink + "\">&laquo;" + prePageForPagerJS + "</a></li>";
    }
    var prev10 = CalculateCurrentPrevPage(pageIndex, pageCount, min, pageSize);

    if (ShowPre10Button(min)) {
        tempLink = StringFormat(baseLink, prev10, pageSize);
        pagerHtml += "<li>";
        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"" + tempLink + "\">" + "..." + "</a>&nbsp;";
        pagerHtml += "</li>";
    }

    for (var i = min; i < max; i++) {
        var styleText = "inactive";
        if (i == pageIndex) {
            styleText = "active";
        }
        tempLink = StringFormat(baseLink, i, pageSize);
        pagerHtml += "<li>";
        pagerHtml += "<a style=\"cursor:pointer\" class=\"" + styleText + "\" onclick=\"" + tempLink + "\">" + (i + 1) + "</a>&nbsp;";
        pagerHtml += "</li>";
    }

    var next10 = CalculateCurrentNextPage(pageIndex, pageCount, max, pageSize);

    if (ShowNext10Button(max, pageCount)) {
        tempLink = StringFormat(baseLink, next10, pageSize);
        pagerHtml += "<li>";
        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"" + tempLink + "\">" + "..." + "</a>&nbsp;";
        pagerHtml += "</li>";
    }

    if (pageIndex >= 0 && pageIndex < pageCount - 1) {
        tempLink = StringFormat(baseLink, pageIndex + 1, pageSize);
        pagerHtml += "<li class=\"next\">";
        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"" + tempLink + "\">" + nextPageForPagerJS + "&raquo;</a></li>";
    }
    var pagerInfo = pagerInfoForPagerJS;
    pagerInfo = StringFormat(pagerInfo, totalCount, pageCount);
    $("#pager").html(pagerHtml);
    $("#headPagerInfo").html(pagerInfo.split(',')[0]);
}

function ShowPre10Button(min) {
    return (min > 0);
}

function ShowNext10Button(max, pageCount) {
    return (max < pageCount);
}

function CalculateCurrentNextPage(pageIndex, pageCount, max, pageSize) {
    var r;
    if (pageIndex + pageSize > pageCount - 1) {
        r = pageCount - 1;
    } else {
        r = pageIndex + pageSize;
    }
    return r;
}

function CalculateCurrentPrevPage(pageIndex, pageCount, min, pageSize) {
    var r;
    if (pageIndex - pageSize < 0) {
        r = 0;
    } else {
        r = pageIndex - pageSize;
    }
    return r;
}

function CalculateCurrentMin(pageIndex, pageCount) {
    var offset = 5;
    if (pageIndex - offset <= 0)
        return 0;
    return pageIndex - offset;
}

function CalculateCurrentMax(pageIndex, pageCount, currentMin) {
    var offset = 10;
    if (currentMin + offset >= pageCount)
        return pageCount;
    return currentMin + offset;
}

function StringFormat(str) {
    for (var i = 1; i < arguments.length; i++) {
        str = str.replace('{' + (i - 1) + '}', arguments[i]);
    }
    return str;
}

function UpdatePagerParamAndCallShowResult(pageIndex, pageSize) {
    gPageIndex = pageIndex;
    gPageSize = pageSize;

    ShowResult(gPageIndex, gPageSize);
}