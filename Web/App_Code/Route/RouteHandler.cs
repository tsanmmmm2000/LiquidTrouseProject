using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// RouteHandler 的摘要描述
/// </summary>
public class RouteHandler
{
	public RouteHandler()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    private static readonly List<RouteRule> _routeRules = new List<RouteRule>();
    public static List<RouteRule> RouteRules
    {
        get { return _routeRules; }
    }

    static RouteHandler()
    {
        var totalActions = new[] {
                RouteConstants.DefaultDefault,
                RouteConstants.Default,
                RouteConstants.Article,
                RouteConstants.TagSearchDefault,
                RouteConstants.TagSearch,
                RouteConstants.KeywordSearchDefault,
                RouteConstants.KeywordSearch,
                RouteConstants.ListDefault,
                RouteConstants.List,
                RouteConstants.Create,
                RouteConstants.Edit,
                RouteConstants.Login,
                RouteConstants.Logout,
                RouteConstants.Error,
                RouteConstants.About
            };

        foreach (var action in totalActions)
        {
            var routeRule = new RouteRule();
            routeRule.RouteName = action;
            routeRule.RouteUrl = RouteConstants.UrlMapping[action];
            routeRule.PhysicalFile = RouteConstants.PhysicalFileMapping[action];
            _routeRules.Add(routeRule);
        }
    }
}