using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Net;

public partial class readarticle : System.Web.UI.Page
{
    private IArticleService _articleService;
    private ITagService _tagService;
    private IHitService _hitService;
    private UserInfo _currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        var factory = WebUtility.Repository;
        _articleService = factory.GetArticleService();
        _tagService = factory.GetTagService();
        _hitService = factory.GetHitService();
        _currentUser = WebUtility.GetCurrentUser();
        RegistHiddenField();
    }

    private void RegistHiddenField()
    {
        var articleId = WebUtility.GetRoutingIntegerParameter(this.Page, "articleid");
        var articleInfo = _articleService.Get(_currentUser, articleId);
        CheckCoverImage(articleInfo);
        ClientScript.RegisterHiddenField("articleInfo", WebUtility.SerializeToJson(articleInfo));

        var tagInfos = _tagService.GetByArticle(_currentUser, articleId);
        ClientScript.RegisterHiddenField("tagInfos", WebUtility.SerializeToJson(tagInfos));

        CreateArticleHit(articleInfo);
    }
    private string GetIPAddress()
    {
        var clientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        return !String.IsNullOrEmpty(clientIP) ? clientIP : Request.ServerVariables["REMOTE_ADDR"];
    }
    private void CheckCoverImage(ArticleInfo articleInfo)
    {
        HttpWebResponse response = null;
        var defaultCoverImageUrl = Request.ApplicationPath + "/image/banner_3.jpg";
        try
        {
            var requestUri = articleInfo.CoverImageUrl;
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "HEAD";
            response = (HttpWebResponse)request.GetResponse();
        }
        catch
        {
            articleInfo.CoverImageUrl = defaultCoverImageUrl;
        }
        finally
        {
            if (response != null)
            {
                response.Close();
            }
        }
    }
    private void CreateArticleHit(ArticleInfo articleInfo)
    {
        var hitInfo = new HitInfo();
        hitInfo.ResourceId = articleInfo.ArticleId;
        hitInfo.IPAddress = GetIPAddress();
        hitInfo.CreationDatetime = DateTime.UtcNow;
        _hitService.CreateArticleHit(_currentUser, hitInfo);

        var hitCount = _hitService.GetArticleHitCount(_currentUser, articleInfo.ArticleId);
        ClientScript.RegisterHiddenField("hitCount", hitCount.ToString());
    }
}