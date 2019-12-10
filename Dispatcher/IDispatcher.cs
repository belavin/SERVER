
using System;
using System.Net.Sockets;
using TcpIPApp.Logger;

namespace TcpIPApp.Dispatcher
{
  internal interface IDispatcher
  {
    void Dispatch(TcpListener listener, ILogger logger, Func<string, string> workload);
  }
}