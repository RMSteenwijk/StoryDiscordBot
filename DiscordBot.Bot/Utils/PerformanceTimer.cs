using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace DiscordBot.Bot.Utils
{
    public class PerformanceTimer<T> : IDisposable
    {
        private readonly string _category;
        private readonly Stopwatch _stopwatch;
        private ILogger<T> _logger;


        public PerformanceTimer(ILogger<T> logger, string category = null)
        {
            _category = category;
            _logger = logger;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            if (string.IsNullOrWhiteSpace(_category))
            {
                _logger.LogInformation($"Execution took {_stopwatch.Elapsed}.");
            }
            else
            {
                _logger.LogInformation($"Execution took {_stopwatch.Elapsed}.", _category);
            }
        }
    }
}
