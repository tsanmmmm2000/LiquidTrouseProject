using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.DTOConverter
{
    public class HitConverter
    {
        public HitConverter() { }

        public IList ToDomainObject(HitInfo[] hitInfos, HitType hitType)
        {
            var hitList = new List<Hit>();
            if (hitInfos != null && hitInfos.Length > 0)
            {
                var articleConverter = new ArticleConverter();
                foreach (var hitInfo in hitInfos)
                {
                    var hit = new Hit();
                    hit.HitId = hitInfo.HitId;
                    hit.ResourceId = hitInfo.ResourceId;
                    hit.IPAddress = hitInfo.IPAddress;
                    hit.HitType = hitType;
                    hit.CreationDatetime = hitInfo.CreationDatetime;
                    hitList.Add(hit);
                }
            }
            return hitList;
        }

        public Hit ToDomainObject(HitInfo hitInfo, HitType hitType)
        {
            var hit = ToDomainObject(new HitInfo[] { hitInfo }, hitType)[0] as Hit;
            return hit;
        }

        public HitInfo[] ToDataTransferObject(IList hits)
        {
            var hitInfoList = new List<HitInfo>();
            if (hits != null && hits.Count > 0)
            {
                var articleConverter = new ArticleConverter();
                foreach (Hit hit in hits)
                {
                    var hitInfo = new HitInfo();
                    hitInfo.HitId = hit.HitId;
                    hitInfo.ResourceId = hit.ResourceId;
                    hitInfo.IPAddress = hit.IPAddress;
                    hitInfo.CreationDatetime = hitInfo.CreationDatetime;
                    hitInfoList.Add(hitInfo);
                }
            }
            return hitInfoList.ToArray();
        }

        public HitInfo ToDataTransferObject(Hit hit)
        {
            return ToDataTransferObject(new List<Hit>() { hit })[0];
        }
    }
}
