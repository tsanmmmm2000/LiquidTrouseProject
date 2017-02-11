using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiquidTrouse.Core.Blog.DataAccess.Domain;

namespace LiquidTrouse.Core.Blog.Service
{
    public interface IHitService
    {
        int GetTagHitCount(UserInfo userInfo, int tagId);
        int GetArticleHitCount(UserInfo userInfo, int articleId);
        void CreateArticleHit(UserInfo userInfo, HitInfo hitInfo);
        void CreateTagHit(UserInfo userInfo, HitInfo hitInfo);
    }
}
