﻿using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class service_ajaxsearchservice : System.Web.UI.Page
{
    private const string KEYWORD_SEARCH = "keyword";
    private const string TAG_SEARCH = "tag";

    private ISearchService _searchService;
    private ITagService _tagService;
    private IHitService _hitService;
    private RepositoryFactory _factory;
    private UserInfo _currentUser;

    private string _keyword;
    private string _sortBy;
    private string _sortDirection;
    private string _tagDisplayName;
    private int _pageIndex;
    private int _pageSize;

    protected void Page_Load(object sender, EventArgs e)
    {
        _factory = WebUtility.Repository;
        _searchService = _factory.GetSearchService();
        _tagService = _factory.GetTagService();
        _hitService = _factory.GetHitService();
        _currentUser = WebUtility.GetCurrentUser();

        _keyword = WebUtility.GetStringParameter("keyword", string.Empty);
        _sortBy = WebUtility.GetStringParameter("sortby", "creationdatetime");
        _sortDirection = WebUtility.GetStringParameter("sortdirection", "desc");
        _tagDisplayName = WebUtility.GetStringParameter("tag", string.Empty);
        _pageIndex = WebUtility.GetIntegerParameter("pageindex", 0);
        _pageSize = WebUtility.GetIntegerParameter("pagesize", 10);

        string command = WebUtility.GetAjaxCommand();
        ProcessCommand(command);
    }

    private void ProcessCommand(string command)
    {
        switch (command)
        {
            case KEYWORD_SEARCH:
                KeywordSearch();
                break;
            case TAG_SEARCH:
                TagSearch();
                break;
            default:
                break;
        }
    }

    private void KeywordSearch()
    {
        try
        {
            var queryPackage = new QueryPackage();
            queryPackage.QueryString = HttpUtility.HtmlDecode(_keyword);
            var sortingInfo = new SortingInfo()
            {
                SortBy = SortingInfo.ParseSortBy(_sortBy),
                SortDirection = SortingInfo.ParseSortDirection(_sortDirection)
            };
			queryPackage.SortingInfo = sortingInfo;
            var articleInfos = _searchService.Search(_currentUser, queryPackage, _pageIndex, _pageSize);
            WebUtility.WriteAjaxResult(true, null, articleInfos);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }
    private void TagSearch()
    {
        try
        {
            var tagDisplayName = HttpUtility.HtmlDecode(_tagDisplayName);
            var tagInfo = _tagService.GetByName(_currentUser, tagDisplayName);
            CreateTagHit(tagInfo);
            var articleInfos = _tagService.GetArticlesByTag(_currentUser, tagInfo.TagId, _pageIndex, _pageSize);
            WebUtility.WriteAjaxResult(true, null, articleInfos);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }

    private string GetIPAddress()
    {
        var clientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        return !String.IsNullOrEmpty(clientIP) ? clientIP : Request.ServerVariables["REMOTE_ADDR"];
    }
    private void CreateTagHit(TagInfo tagInfo)
    {
        var hitInfo = new HitInfo();
        hitInfo.ResourceId = tagInfo.TagId;
        hitInfo.IPAddress = GetIPAddress();
        hitInfo.CreationDatetime = DateTime.UtcNow;
        _hitService.CreateTagHit(_currentUser, hitInfo);
    }
}