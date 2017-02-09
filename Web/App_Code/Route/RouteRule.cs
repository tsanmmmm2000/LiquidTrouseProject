using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// RouteRule 的摘要描述
/// </summary>
public class RouteRule
{
	public RouteRule()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public RouteRule(string routeName, string routeUrl, string physicalFile)
    {
        _routeName = routeName;
        _routeUrl = routeUrl;
        _physicalFile = physicalFile;
    }

    private string _routeName;
    private string _routeUrl;
    private string _physicalFile;

    public string RouteName
    {
        get { return _routeName; }
        set { _routeName = value; }
    }

    public string RouteUrl
    {
        get { return _routeUrl; }
        set { _routeUrl = value; }
    }

    public string PhysicalFile
    {
        get { return _physicalFile; }
        set { _physicalFile = value; }
    }
}