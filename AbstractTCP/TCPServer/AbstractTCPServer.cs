using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AbstractTCP.TCPServer
{
    public abstract class AbstractTCPServer
    {
        private const int SEC = 1000; //Easily defined second
        private bool running = true;
        private readonly List<Task> clients = new List<Task>();
        private readonly IPAddress ListenOnIp = IPAddress.Any;

        public int PORT { get; private set; }
        public int STOPPORT { get; private set; }
        public string NAME { get; private set; }

        /// <summary>
        /// Setup of an abstract TCP server
        /// </summary>
        /// <param name="port">The port number</param>
        /// <param name="name">The server name</param>
        protected ServerConfig Config { get; private set; } = ServerConfig.Instance;
        
        public AbstractTCPServer(int port, string name) : this(port, port + 1, name)
        {

        }

        public AbstractTCPServer(int port, int stopport, string name)
        {
            Config.ServerPort = port;
            Config.ShutDownPort = stopport;
            Config.ServerName = name;
        }
        public AbstractTCPServer(String filename = "ServerConfig.xml")
        {
            string configpath = Environment.GetEnvironmentVariable("AbstractServerConfig");

            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(configpath + filename);

            XmlNode? portNode = configDoc.DocumentElement?.SelectSingleNode("ServerPort");
            if (portNode != null)
            {
                String portStr = portNode.InnerText.Trim();
                int port = Convert.ToInt32(portStr);
                Config.ServerPort = port;
            }

            XmlNode? stopportNode = configDoc.DocumentElement?.SelectSingleNode("StopServerPort");
            if (stopportNode != null)
            {
                String stopportStr = stopportNode.InnerText.Trim();
                int stopport = Convert.ToInt32(stopportStr);
                Config.ShutDownPort = stopport;
            }

            XmlNode? nameNode = configDoc?.DocumentElement?.SelectSingleNode("ServerName");
            if (nameNode != null)
            {
                String name = nameNode.InnerText.Trim();
                Config.ServerName = name;
            }
        }
        /// <summary>
        /// Initializes both the server and the stop server
        /// Looks for clients and prints the port number and name (part of a previous assignment)
        /// </summary>
        public void Start()
        {
            Task.Run(TheStopServer);

            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine($"Server started at port: {PORT}");
            Console.WriteLine($"Server name: {NAME}");

            while (running)
            {
                if (listener.Pending()) //Do we have a client?
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client found");
                    clients.Add(
                        Task.Run(() =>
                        {
                            TcpClient tmpClient = client;
                            DoOneClient(client);
                        })
                    );
                }
                else //We don't have a client, sleep for 3 seconds
                {
                    Thread.Sleep(3 * SEC);
                }
            }
            Task.WaitAll(clients.ToArray());
            Console.WriteLine("Server stopped");
        }

        public void DoOneClient(TcpClient sock)
        {
            using (StreamReader sr = new StreamReader(sock.GetStream()))
            using (StreamWriter sw = new StreamWriter(sock.GetStream()))
            {
                sw.AutoFlush = true;
                Console.WriteLine("Handle one client");

                TemplateMethod(sr, sw);
            }
        }
        protected abstract void TemplateMethod(StreamReader sr, StreamWriter sw);

        private void StoppingServer()
        {
            running = false;
        }

        private void TheStopServer()
        {
            TcpListener listener = new TcpListener(ListenOnIp, STOPPORT);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();

            StoppingServer();
            client?.Close();
            listener?.Stop();
        }
    }
}
