using LiquidTrouse.Core.Blog.Service;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LiquidTrouse.Core.Globalization;

public partial class management_editarticle : System.Web.UI.Page
{
    private IArticleService _articleService;
    private ITagService _tagService;
    private UserInfo _currentUser;

    protected i18nHelper i18n;

    protected int ArticleId;

    protected void Page_Load(object sender, EventArgs e)
    {
        var factory = WebUtility.Repository;
        _currentUser = WebUtility.GetCurrentUser();
        _articleService = factory.GetArticleService();
        _tagService = factory.GetTagService();
        ArticleId = WebUtility.GetRoutingIntegerParameter(this.Page, "articleid");
        i18n = new i18nHelper();
        RegistHiddenField();
    }

    private void RegistHiddenField()
    {
        var articleInfo = _articleService.Get(_currentUser, ArticleId);
        ClientScript.RegisterHiddenField("articleInfo", WebUtility.SerializeToJson(articleInfo));

        var tagInfos = _tagService.GetByArticle(_currentUser, ArticleId);
        ClientScript.RegisterHiddenField("tagInfos", WebUtility.SerializeToJson(tagInfos));
    }
}