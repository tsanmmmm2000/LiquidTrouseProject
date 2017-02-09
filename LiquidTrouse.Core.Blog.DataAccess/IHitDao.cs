using LiquidTrouse.Core.Blog.DataAccess.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess
{
    public interface IHitDao
    {
        Hit GetLatestByIPAddress(string ipAddress, int resourceId, HitType hitType);
        IList GetTopN(int topN, HitType hitType);
        int GetCount(int resourceId, HitType hitType);
        void Create(Hit hit);
        void Update(Hit hit);
        void Delete(int resourceId, HitType hitType);
    }
}
