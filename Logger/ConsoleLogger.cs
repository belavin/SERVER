
using System;

namespace TcpIPApp.Logger
{
  internal class ConsoleLogger : BaseLogger, ILogger
  {
    protected override void write(string line)
    {
      if (this.compact)
        Console.Write(line);
      else
        Console.WriteLine(line);
    }
  }
}