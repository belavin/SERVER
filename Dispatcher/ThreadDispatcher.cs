
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using TcpIPApp.Logger;
using TcpIPApp.Server;

namespace TcpIPApp.Dispatcher
{
  internal class ThreadDispatcher : IDispatcher
  {
        public string current_path;
        public string file1;
        public string date_year;
        public string date_month;
        public string date_day;


        public void Dispatch(TcpListener listener, ILogger logger, Func<string, string> workload)
    {
      while (true)
      {
        try
        {
          TcpClient tcpClient = listener.AcceptTcpClient();
          SyncServer server = new SyncServer(tcpClient, logger, workload);

          Thread thread = new Thread(new ThreadStart(server.Run));
          thread.Start();
                    Console.WriteLine("������ ������� � ������ ���������");
                    date_year = DateTime.Now.ToString("yyyy");
                    date_month = DateTime.Now.ToString("MM");
                    date_day = DateTime.Now.ToString("dd");

                    Console.WriteLine("������� ��� : " + date_year);
                    Console.WriteLine("������� ����� : " + date_month);
                    Console.WriteLine("������� ���� : " + date_day);
                    
                }
        catch (Exception ex)
        {
          logger.Log(LoggerThreshold.Error, ex.Message);
        }
      }
    }


    }
}