using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Web;

public partial class service_ajaxarticleservice : System.Web.UI.Page
{
    private const string CREATE = "create";
    private const string DELETE = "delete";
    private const string UPDATE = "update";
    private const string BATCH_GET = "batchget";
    private const string GET = "get";

    private IArticleService _articleService;
    private RepositoryFactory _factory;
    private UserInfo _currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        _factory = WebUtility.Repository;
        _articleService = _factory.GetArticleService();
        _currentUser = WebUtility.GetCurrentUser();
        string command = WebUtility.GetAjaxCommand();
        ProcessCommand(command);
    }

    private void ProcessCommand(string command)
    {
        switch (command)
        {
            case CREATE:
                Create();
                break;
            case UPDATE:
                Update();
                break;
            case DELETE:
                Delete();
                break;
            case BATCH_GET:
                BatchGet();
                break;
            case GET:
                Get();
                break;
            default:
                break;
        }
    }

    private void Create()
    {
        var title = WebUtility.GetStringParameter("title", string.Empty);
        var urlTitle = WebUtility.GetStringParameter("urltitle", string.Empty);
        var content = WebUtility.GetStringParameter("content", string.Empty);
        var coverImageUrl = WebUtility.GetStringParameter("coverimageurl", string.Empty);
        var tags = WebUtility.GetStringParameter("tags", string.Empty);
        var creationDatetime = WebUtility.GetStringParameter("creationdatetime", string.Empty);
        try
        {
            tags = CleanTagSpace(HttpUtility.HtmlDecode(tags));
            var tagDisplayNames = tags.Split(',');

            var articleInfo = new ArticleInfo();
            articleInfo.UserId = _currentUser.UserId;
            articleInfo.Content = HttpUtility.HtmlDecode(content);
            articleInfo.Title = HttpUtility.HtmlDecode(title);
            articleInfo.UrlTitle = urlTitle;
            articleInfo.CoverImageUrl = coverImageUrl;
            articleInfo.CreationDatetime = (!String.IsNullOrEmpty(creationDatetime))
                ? WebUtility.ParseISO8601SimpleString(creationDatetime)
                : DateTime.UtcNow; 
            articleInfo.LastModifiedDatetime = DateTime.UtcNow;

            _articleService.Create(_currentUser, articleInfo, tagDisplayNames);

            WebUtility.WriteAjaxResult(true, string.Empty, null);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }
    private void Update()
    {
        var articleId = WebUtility.GetIntegerParameter("articleid", -1);
        var title = WebUtility.GetStringParameter("title", string.Empty);
        var urlTitle = WebUtility.GetStringParameter("urltitle", string.Empty);
        var content = WebUtility.GetStringParameter("content", string.Empty);
        var coverImageUrl = WebUtility.GetStringParameter("coverimageurl", string.Empty);
        var tags = WebUtility.GetStringParameter("tags", string.Empty);
        var creationDatetime = WebUtility.GetStringParameter("creationdatetime", string.Empty);
        try
        {
            tags = CleanTagSpace(HttpUtility.HtmlDecode(tags));
            var tagDisplayNames = tags.Split(',');

            var articleInfo = new ArticleInfo();
            articleInfo.ArticleId = articleId;
            articleInfo.UserId = _currentUser.UserId;
            articleInfo.Content = HttpUtility.HtmlDecode(content);
            articleInfo.Title = HttpUtility.HtmlDecode(title);
            articleInfo.UrlTitle = urlTitle;
            articleInfo.CoverImageUrl = coverImageUrl;
            articleInfo.CreationDatetime = (!String.IsNullOrEmpty(creationDatetime))
                ? WebUtility.ParseISO8601SimpleString(creationDatetime)
                : DateTime.UtcNow;
            articleInfo.LastModifiedDatetime = DateTime.UtcNow;

            _articleService.Update(_currentUser, articleInfo, tagDisplayNames);

            WebUtility.WriteAjaxResult(true, string.Empty, null);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }
    private void Delete() 
    {
        var articleId = WebUtility.GetIntegerParameter("articleid", -1);
        try
        {
            _articleService.Delete(_currentUser, articleId);

            WebUtility.WriteAjaxResult(true, string.Empty, null);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }
    private void BatchGet()
    {
        var pageIndex = WebUtility.GetIntegerParameter("pageindex", 0);
        var pageSize = WebUtility.GetIntegerParameter("pagesize", 10);
        try
        {
            var articleInfos = _articleService.Get(_currentUser, pageIndex, pageSize);
            WebUtility.WriteAjaxResult(true, null, articleInfos);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }
    private void Get()
    {
        var articleId = WebUtility.GetIntegerParameter("articleid");
        try
        {
            var articleInfo = _articleService.Get(_currentUser, articleId);
            WebUtility.WriteAjaxResult(true, null, articleInfo);
        }
        catch (Exception ex)
        {
            WebUtility.WriteTranslatedAjaxError(ex);
        }
    }

    private string CleanTagSpace(string tags)
    {
        var tagArray = tags.Trim().Split(',');
        var cleanTags = string.Empty;
        for (var i = 0; i < tagArray.Length; i++)
        {
            if (tagArray[i].Trim() != string.Empty)
            {
                if (cleanTags.Length > 0)
                {
                    cleanTags += "," + tagArray[i];
                }
                else
                {
                    cleanTags = tagArray[i];
                }
            }
        }
        return cleanTags;
    }
}