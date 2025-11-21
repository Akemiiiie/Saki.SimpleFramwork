using NLog;
using NLog.Config;
using System;

namespace Saki.Logging
{
    public class NLogLogger : ILoggerService
    {
        private readonly Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
            var config = new XmlLoggingConfiguration("NLog.config");
            LogManager.Configuration = config;
        }

        public void Info(string message) => _logger.Info(message);

        public void Warn(string message) => _logger.Warn(message);

        public void Error(string message, Exception ex = null) =>
            _logger.Error(ex, message);
    }
}
