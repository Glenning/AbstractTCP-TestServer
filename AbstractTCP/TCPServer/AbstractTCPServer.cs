using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public AbstractTCPServer(int port, string name) : this(port, port + 1, name)
        {

        }

        public AbstractTCPServer(int port, int stopport, string name)
        {
            PORT = port;
            STOPPORT = stopport;
            NAME = name;
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
