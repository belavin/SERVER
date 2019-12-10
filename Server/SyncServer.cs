
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using TcpIPApp.Dispatcher;
using TcpIPApp.Logger;

namespace TcpIPApp.Server
{
  internal class SyncServer
  {
        public string current_path;
        public string dirName;
        public String file1;
        public StreamWriter wrstream ;

        public SyncServer(TcpClient tcpClient, ILogger logger, Func<string, string> workload)
    {
      this.tcpClient = tcpClient;
      this.logger = logger;
      this.workload = workload;
    }

    public void Run()
    {
      try
      {

                
        using (wrstream = new StreamWriter(this.tcpClient.GetStream()))
        {
                    //  var buffer = new byte[256];
                    //  var sb = new StringBuilder();
                    //  do
                    //  {
                    //   var bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                    //   sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    //  }
                    //  while (networkStream.DataAvailable);
                    string date_year = DateTime.Now.ToString("yyyy");
                    string date_month = DateTime.Now.ToString("MM");
                    string date_day = DateTime.Now.ToString("dd");

                    dirName = "D:\\BagVision\\Client\\Archive\\";
                    //current_path = "D:\\example";
                    int x = Int32.Parse(date_day);

                    if (x > 0 && x < 10)
                    {
                        current_path = dirName + "\\" + date_year + "\\" + date_month.Remove(0, 1) + "\\" + "0" + date_day;
                    }
                    else
                    {
                        current_path = dirName + "\\" + date_year + "\\" + date_month.Remove(0, 1) + "\\" + date_day;
                    }


                    try
                    {

                        FileSystemWatcher watcher = new FileSystemWatcher();
                        watcher.Path = @current_path;

                        watcher.NotifyFilter = NotifyFilters.FileName;
                        /*
                        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                        */
                        watcher.Filter = "*.sxr";

                        // Add event handlers.
                        //watcher.Changed += new FileSystemEventHandler(OnChanged);
                        watcher.Created += new FileSystemEventHandler(OnChanged);
                        //watcher.Deleted += new FileSystemEventHandler(OnChanged);
                        // watcher.Renamed += new RenamedEventHandler(OnRenamed);

                        // Begin watching.
                        watcher.EnableRaisingEvents = true;

                    }
                    catch (Exception m)
                    {
                        Console.WriteLine("Не могу найти " + current_path);
                        //exeption_filewatcher_state = 1;
                    }

                    //          string input = sb.ToString();
                    //this.logger.Cache(LoggerThreshold.Debug, $"Server received [{input}]");

                    //string output = this.workload(input);

                    //byte[] sendBuffer = Encoding.UTF8.GetBytes(output);
                    // networkStream.Write(sendBuffer, 0, sendBuffer.Length);

                    // this.logger.Cache(LoggerThreshold.Debug, $"Server send [{output}]");

                    // if ((int)this.logger.LogThreshold <= (int)LoggerThreshold.Minimal)
                    //   this.logger.Cache(LoggerThreshold.Minimal, ".");
                    while (true)
                    {

                    }
                }
      }
      catch (SocketException se)
      {
        this.logger.Cache(LoggerThreshold.Error, se.ErrorCode + ": " + se.Message);
      }
      catch (IOException io)
      {
        this.logger.Cache(LoggerThreshold.Error, io.Data + ": " + io.Message);
      }

      this.tcpClient.Close();
      this.logger.Flush();
    }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            Thread.Sleep(500);
            Byte[] bytes = File.ReadAllBytes(e.FullPath);

            file1 = Convert.ToBase64String(bytes);
            string date_year = DateTime.Now.ToString("yyyy");
            string date_month = DateTime.Now.ToString("MM");
            string date_day = DateTime.Now.ToString("dd");
            string current_date = date_year + date_month + date_day + "-";
            StringBuilder sb = new StringBuilder();
            System.Xml.XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartElement("date");
            xw.WriteAttributeString("xml_date", "yyyy-mm-dd", null, current_date);
            xw.WriteStartElement("log");

            xw.WriteValue(file1);
            xw.WriteEndElement();


            xw.WriteEndElement();



            xw.Close();

            try
            {
               // wrstream.WriteLine(file1);
                wrstream.WriteLine(sb.ToString());
                wrstream.Flush();
            }
            catch (Exception ecp)
            {
                ecp.GetBaseException();
            }
            try
            {
                //ns.Flush();
                wrstream.Flush();
                //wrstream.Close(); //если я закрываю поток происходит передача данных и клиент теряет связь 
                //Я бы не говорил что это очень плохо
            }
            catch (Exception ex2)
            {
                ex2.GetBaseException();
            }
        }

        private TcpClient tcpClient;
    private ILogger logger;
    private Func<string, string> workload;
  }
}