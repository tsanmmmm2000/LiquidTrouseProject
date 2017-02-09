using log4net;
using Spring.Context;
using Spring.Context.Support;
using System;

namespace LiquidTrouse.Core
{
    public class Utility
    {
        private static IApplicationContext _ctx;
        private static readonly ILog Logger = LogManager.GetLogger("LT");

        public static IApplicationContext ApplicationContext
        {
            get
            {
                IApplicationContext currentCtx = ContextRegistry.GetContext();
                if (_ctx == null)
                {
                    _ctx = currentCtx;
                }
                else
                {
                    if (!Object.ReferenceEquals(_ctx, currentCtx))
                    {
                        _ctx = currentCtx;
                    }
                }
                return _ctx;
            }
            set
            {
                _ctx = value;
            }
        }
        public static void DebugLog(string message, Exception ex)
        {
            if (Logger.IsDebugEnabled)
                Logger.Debug(message, ex);
        }
        public static void DebugLog(string message)
        {
            if (Logger.IsDebugEnabled)
                Logger.Debug(message);
        }
        public static void ErrorLog(string message, Exception ex)
        {
            if (Logger.IsErrorEnabled)
                Logger.Error(message, ex);
        }
        public static void ErrorLog(string message)
        {
            if (Logger.IsErrorEnabled)
                Logger.Error(message);
        }
    }
}
