﻿
using System;
using System.Net;
using System.Net.Sockets;
using TcpIPApp.Dispatcher;
using TcpIPApp.Logger;
using TcpIPApp.WorkLoad;

namespace TcpIPApp.Server
{
  internal class Runner
  {
    public string ip_addr_comp;

    public int Run(Options options)
    {
      this.options = options;
      this.parseBaseElements();

      runSyncServer(options);

      return 0;
    }

    private void runSyncServer(Options options)
    {
      // Получение имени компьютера.
      String host = System.Net.Dns.GetHostName();
      // Получение ip-адреса.
      
      System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];

      TcpListener listener = new TcpListener(ip, options.ServerVerb.Port);
      listener.Start();

      this.dispatcher.Dispatch(listener, this.logger, this.workload.Workload);
    }


    private void parseBaseElements()
    {
      ServerSubOptions srvOptions = this.options.ServerVerb;

      switch (srvOptions.Dispatcher)
      {
        case DispatcherType.Thread:
          this.dispatcher = new ThreadDispatcher();
          break;

        case DispatcherType.Pool:
          this.dispatcher = new PoolDispatcher(srvOptions.NumOfConcurentProceses);
          break;

        case DispatcherType.Dynamic:
          this.dispatcher = new DynamicDispatcher(srvOptions.NumOfConcurentProceses);
          break;
      }

      switch (srvOptions.Workload)
      {
        case WorkloadType.Echo:
          this.workload = new Echo(srvOptions.MinWorkloadTime, srvOptions.MaxWorkloadTime, srvOptions.RealWork);
          break;

        case WorkloadType.Time:
          this.workload = new Time(srvOptions.MinWorkloadTime, srvOptions.MaxWorkloadTime, srvOptions.RealWork);
          break;
      }

      this.logger = (string.IsNullOrEmpty(srvOptions.LogFileName)) ?
        (ILogger)(new ConsoleLogger()) :
        (ILogger)(new FileLogger(srvOptions.LogFileName));
      this.logger.LogThreshold = srvOptions.LoggerThreshold;

      if (srvOptions.RealWork)
        this.logger.Log(LoggerThreshold.Debug, "real work");
      else
        this.logger.Log(LoggerThreshold.Debug, "NOT real work");
    }

    private Options options;
    private IDispatcher dispatcher;
    private IWorkload workload;
    private ILogger logger;
  }
}