using Microsoft.Extensions.Logging;
using Prism.Logging;

namespace Calculator.Wpf
{
    class PrismLogger : ILoggerFacade
    {
        private readonly ILogger<PrismLogger> _logger;

        public PrismLogger(ILogger<PrismLogger> logger)
        {
            _logger = logger;
        }

        public void Log(string message, Category category, Priority priority)
        {
            var level = category switch
            {
                Category.Debug => LogLevel.Debug,
                Category.Exception => LogLevel.Error,
                Category.Info => LogLevel.Information,
                Category.Warn => LogLevel.Warning,
                _ => LogLevel.None
            };

            _logger.Log(level, message);
        }
    }
}
