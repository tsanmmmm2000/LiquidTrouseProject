using LiquidTrouse.Core.CacheService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.CacheService
{
    public delegate bool CacheKeyMatchEvaluator(CacheKey cacheKey);
}
