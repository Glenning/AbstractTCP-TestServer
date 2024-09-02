using AbstractTCP.TCPServer;
using TestServer.Div;
using TestServer.Server;

String path = Environment.GetEnvironmentVariable("AbstractServerConfig"); //Added the Xml as an environment var

MyServer server = new MyServer();
server.Start();

XmlConfig conf = new XmlConfig(); //Uses the name of the file that sets up the use of Xml
conf.Read(path);

Console.ReadKey();