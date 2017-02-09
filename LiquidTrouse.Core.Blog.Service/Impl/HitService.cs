using LiquidTrouse.Core.Blog.DataAccess;
using LiquidTrouse.Core.Blog.Service.DTO;
using LiquidTrouse.Core.Blog.Service.DTOConverter;
using LiquidTrouse.Core.AccountManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiquidTrouse.Core.Blog.DataAccess.Domain;

namespace LiquidTrouse.Core.Blog.Service.Impl
{
    public class HitService : IHitService
    {
        private HitConverter _converter = new HitConverter();

        private IHitDao _hitDao;
        public IHitDao HitDao
        {
            set { _hitDao = value; }
        }

        public HitInfo[] GetArticleTopN(UserInfo userInfo, int topN)
        {
            return GetTopN(topN, HitType.Article);
        }
        public HitInfo[] GetTagTopN(UserInfo userInfo, int topN)
        {
            return GetTopN(topN, HitType.Tag);
        }
        public int GetTagHitCount(UserInfo userInfo, int tagId)
        {
            return _hitDao.GetCount(tagId, HitType.Tag);
        }
        public int GetArticleHitCount(UserInfo userInfo, int articleId)
        {
            return _hitDao.GetCount(articleId, HitType.Article);
        }
        public void CreateArticleHit(UserInfo userInfo, HitInfo hitInfo)
        {
            CreateHit(hitInfo, HitType.Article);
        }
        public void CreateTagHit(UserInfo userInfo, HitInfo hitInfo)
        {
            CreateHit(hitInfo, HitType.Tag);
        }

        private HitInfo[] GetTopN(int topN, HitType hitType)
        {
            var hits = _hitDao.GetTopN(topN, hitType);
            return _converter.ToDataTransferObject(hits);
        }
        private void CreateHit(HitInfo hitInfo, HitType hitType)
        {
            const int withinHour = 24;

            var hit = _converter.ToDomainObject(hitInfo, hitType);
            var latestHit = _hitDao.GetLatestByIPAddress(hitInfo.IPAddress, hitInfo.ResourceId, hitType);
            if (latestHit != null)
            {
                var latestHitDatetime = latestHit.CreationDatetime;
                if (hitInfo.CreationDatetime.Subtract(latestHitDatetime).TotalHours > withinHour)
                {
                    _hitDao.Create(hit);
                }
            }
            else
            {
                _hitDao.Create(hit);
            }
        }
    }
}
