using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class service_ajaxtagservice : System.Web.UI.Page
{
    private const string GET = "get";
    private const string GET_ALL = "getall";

    private ITagService _tagService;
    private RepositoryFactory _factory;
    private UserInfo _currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        _factory = WebUtility.Repository;
        _tagService = _factory.GetTagService();
        _currentUser = WebUtility.GetCurrentUser();
        string command = WebUtility.GetAjaxCommand();
        ProcessCommand(command);
    }

    private void ProcessCommand(string command)
    {
        switch (command)
        {
            case GET:
                Get();
                break;
            case GET_ALL:
                GetAll();
                break;
            default:
                break;
        }
    }

    private void Get()
    {
        var tagId = WebUtility.GetIntegerParameter("tagId");
        try
        {
            var tagInfo = _tagService.Get(_currentUser, tagId);
            WebUtility.WriteAjaxResult(true, null, tagInfo);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }

    private void GetAll()
    {
        try
        {
            var tagInfos = _tagService.GetAll(_currentUser);
            WebUtility.WriteAjaxResult(true, null, tagInfos);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }

}