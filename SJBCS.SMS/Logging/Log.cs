using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SJBCS.SMS.Logging
{
    public class Log
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void INFO(string message)
        {
            logger.Info(message);
        }

        public static void WARN(string message)
        {
            logger.Warn(message);
        }

        public static void WARN(string message, Exception exception)
        {
            logger.Warn(message, exception);
        }

        public static void ERROR(string message)
        {
            logger.Error(message);
        }

        public static void ERROR(string message, Exception exception)
        {
            logger.Error(message, exception);
        }

        public static void DEBUG(string message)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }
    }
}