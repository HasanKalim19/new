﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Logger
{
    /// <summary>
    /// A class that logs only info level details in the info.log file.
    /// </summary>
    internal class InfoLogger : IInfoLogger
    {
        /// <summary>
        /// Logs the message along with the specified logger.
        /// </summary>
        /// <param name="type">A variable that specifies type of the logger.</param>
        /// <param name="msg">A string containing the message for logging</param>
        /// <param name="methodName">A string containing name of the calling method.(Optional)</param>
        public void LogInfo(Type type, string msg, string methodName = "")
        {
            ILog log = log4net.LogManager.GetLogger(type);
            log.Info((methodName == "" ? "" : methodName + " ===> ") + msg);
        }

        /// <summary>
        /// Logs the message and exception details along with the specified logger.
        /// </summary>
        /// <param name="type">A variable that specifies type of the logger.</param>
        /// <param name="msg">A string containing the message for logging.</param>
        /// <param name="exp">An variable containing "Exception" object for logging.</param>
        /// <param name="methodName">A string containing name of the calling method.(Optional)</param>
        public void LogInfo(Type type, string msg, Exception exp, string methodName = "")
        {
            ILog log = log4net.LogManager.GetLogger(type);
            log.Info((methodName == "" ? "" : methodName + " ===> ") + msg, exp);
        }
    }
}
