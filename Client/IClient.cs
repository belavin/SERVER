
using System.Net;
using TcpIPApp.Logger;

namespace TcpIPApp.Client
{
  internal interface IClient
  {
    string Process(IPEndPoint localEndPoint, ILogger logger, string text);
  }
}