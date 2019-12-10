
namespace TcpIPApp.Logger
{
  internal interface ILogger
  {
    LoggerThreshold LogThreshold { get; set; }

    // log immediately
    void Log(LoggerThreshold threshold, string line);

    // cache log data
    void Cache(LoggerThreshold threshold, string line);

    // flush all cached data and clear cache
    void Flush();
  }
}