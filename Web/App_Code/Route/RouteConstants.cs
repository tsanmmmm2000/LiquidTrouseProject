using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// RouteConstants 的摘要描述
/// </summary>
public class RouteConstants
{
	public RouteConstants()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    #region route name
    public const string DefaultDefault = "DefaultDefault";
    public const string Default = "Default";
    public const string Article = "Article";
    public const string TagSearchDefault = "TagSearchDefault";
    public const string TagSearch = "TagSearch";
    public const string KeywordSearchDefault = "KeywordSearchDefault";
    public const string KeywordSearch = "KeywordSearch";
    public const string ListDefault = "ListDefault";
    public const string List = "List";
    public const string Create = "Create";
    public const string Edit = "Edit";
    public const string Login = "Login";
    public const string Logout = "Logout";
    public const string Error = "Error";
    public const string About = "About";
    #endregion

    #region route url
    public const string DefaultDefaultUrl = "default";
    public const string DefaultUrl = "default/{pageindex}/{pagesize}";
    public const string ArticleUrl = "article/{urltitle}/{articleid}";
    public const string TagSearchDefaultUrl = "search/tag/{tag}";
    public const string TagSearchUrl = "search/tag/{tag}/{pageindex}/{pagesize}";
    public const string KeywordSearchDefaultUrl = "search/keyword/{keyword}";
    public const string KeywordSearchUrl = "search/keyword/{keyword}/{pageindex}/{pagesize}";
    public const string ListDefaultUrl = "management/list";
    public const string ListUrl = "management/list/{pageindex}/{pagesize}";
    public const string CreateUrl = "management/create";
    public const string EditUrl = "management/edit/{articleid}";
    public const string LoginUrl = "management/login";
    public const string LogoutUrl = "management/logout";
    public const string ErrorUrl = "error";
    public const string AboutUrl = "about";
    #endregion

    #region route physical file
    public const string DefaultPhysicalFile = "~/default.aspx";
    public const string ArticlePhysicalFile = "~/readarticle.aspx";
    public const string TagSearchPhysicalFile = "~/searchresult.aspx";
    public const string KeywordSearchPhysicalFile = "~/searchresult.aspx";
    public const string ListPhysicalFile = "~/management/articlelist.aspx";
    public const string CreatePhysicalFile = "~/management/createarticle.aspx";
    public const string EditPhysicalFile = "~/management/editarticle.aspx";
    public const string LoginPhysicalFile = "~/management/login.aspx";
    public const string LogoutPhysicalFile = "~/management/logout.aspx";
    public const string ErrorPhysicalFile = "~/error.aspx";
    public const string AboutPhysicalFile = "~/about.aspx";
    #endregion

    #region mapping
    public static IDictionary<string, string> UrlMapping
    {
        get
        {
            return new Dictionary<string, string>
                {
                    { DefaultDefault , DefaultDefaultUrl },
                    { Default , DefaultUrl },
                    { Article, ArticleUrl  },
                    { TagSearchDefault, TagSearchDefaultUrl },
                    { TagSearch, TagSearchUrl },
                    { KeywordSearchDefault, KeywordSearchDefaultUrl },
                    { KeywordSearch, KeywordSearchUrl },
                    { ListDefault, ListDefaultUrl },
                    { List, ListUrl },
                    { Create, CreateUrl },
                    { Edit, EditUrl },
                    { Login, LoginUrl },
                    { Logout, LogoutUrl },
                    { Error, ErrorUrl },
                    { About, AboutUrl }
                };
        }
    }
    public static IDictionary<string, string> PhysicalFileMapping
    {
        get
        {
            return new Dictionary<string, string>
                {
                    { DefaultDefault , DefaultPhysicalFile },
                    { Default , DefaultPhysicalFile },
                    { Article, ArticlePhysicalFile },
                    { TagSearchDefault, TagSearchPhysicalFile },
                    { TagSearch, TagSearchPhysicalFile },
                    { KeywordSearchDefault, KeywordSearchPhysicalFile },
                    { KeywordSearch, KeywordSearchPhysicalFile },
                    { ListDefault, ListPhysicalFile },
                    { List, ListPhysicalFile },
                    { Create, CreatePhysicalFile },
                    { Edit, EditPhysicalFile },
                    { Login, LoginPhysicalFile },
                    { Logout, LogoutPhysicalFile },
                    { Error, ErrorPhysicalFile },
                    { About, AboutPhysicalFile }
                };
        }
    }
    #endregion

}