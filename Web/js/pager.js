var PagerAgent = function (opts) {
	
    var defaults = {
	    PreviousPage: "Previous",
	    NextPage: "Next",
	    Info: "{0} Articles, {1} Pages Total"
    };
	
    var options = opts || {};

    var settings = $.extend({}, defaults, options);
    var previousPage = settings.PreviousPage;
    var nextPage = settings.NextPage;
    var info = settings.Info;
	
    var WritePager = function (pageIndex, pageSize, totalCount) {
		
        if (pageIndex == null) {
            pageIndex = 0;
        }
        if (pageSize == null) {
            pageSize = 10;
        }

        var pageCount = ((totalCount % pageSize) == 0)
            ? totalCount / pageSize
            : parseInt((totalCount / pageSize).toString(), 10) + 1;

        var minIndex = CalMinIndex(pageIndex);
        var maxIndex = CalMaxIndex(pageIndex, pageCount);

        var pagerHtml = "";
		
        if (pageIndex > 0) {
	        pagerHtml += "<li class=\"previous\">";
	        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"javascript:UpdatePage(" + (pageIndex - 1) + "," + pageSize + ")\">&laquo;" + previousPage + "</a>";
	        pagerHtml += "</li>";
        }

        var showPreviousPageGroup = (minIndex > 0);
        if (showPreviousPageGroup) {
	        pagerHtml += "<li>";
	        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"javascript:UpdatePage(" + (minIndex - 1) + "," + pageSize + ")\">" + "..." + "</a>&nbsp;";
	        pagerHtml += "</li>";
        }
		
        for (var i = minIndex; i <= maxIndex; i++) {
	        var style = (i == pageIndex) ? "active" : "inactive"; 
	        pagerHtml += "<li>";
	        pagerHtml += "<a style=\"cursor:pointer\" class=\"" + style + "\" onclick=\"javascript:UpdatePage(" + i + "," + pageSize + ")\">" + (i + 1) + "</a>&nbsp;";
	        pagerHtml += "</li>";
        }

        var showNextPageGroup = (maxIndex + 1 < pageCount);
        if (showNextPageGroup) {
	        pagerHtml += "<li>";
	        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"javascript:UpdatePage(" + (maxIndex + 1) + "," + pageSize + ")\">" + "..." + "</a>&nbsp;";
	        pagerHtml += "</li>";
        }
		
        if (pageIndex >= 0 && pageIndex + 1 < pageCount) {
	        pagerHtml += "<li class=\"next\">";
	        pagerHtml += "<a style=\"cursor:pointer\" onclick=\"javascript:UpdatePage(" + (pageIndex + 1) + "," + pageSize + ")\">" + nextPage + "&raquo;</a>";
	        pagerHtml += "</li>";
        }

        $("#pager").html(pagerHtml);
    }
    var CalMinIndex = function (pageIndex) {
        var offset = 4;
        var minIndex = pageIndex - offset;
        return (minIndex > 0) ? minIndex : 0;
    }
    var CalMaxIndex = function (pageIndex, pageCount) {
        var offset = 4;
        var maxIndex = pageIndex + offset;
        return (maxIndex + 1 < pageCount) ? maxIndex : pageCount - 1;
    }

    this.WritePager = function (pageIndex, pageSize, pageCount, totalCount) {
        WritePager(pageIndex, pageSize, pageCount, totalCount);
	}
	this.ApplySettings = function (newSettings) {
	    settings = $.extend({}, defaults, newSettings);
	    previousPage = settings.PreviousPage
	    nextPage = settings.NextPage;
	    info = settings.Info;
	};

};
var PagerAgentInstance = new PagerAgent();
function InitPager(settings) {
    PagerAgentInstance.ApplySettings(settings);
}
function WritePager(pageIndex, pageSize, pageCount, totalCount) {
    PagerAgentInstance.WritePager(pageIndex, pageSize, pageCount, totalCount);
}
function UpdatePage(pageIndex, pageSize) {
	gPageIndex = pageIndex;
	gPageSize = pageSize;
	WriteArticles();
}