using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTCP.TCPServer
{
    public class ServerConfig
    {
        private ServerConfig() { }

        private static ServerConfig _instance = new ServerConfig();

        public static ServerConfig Instance => _instance;

        public int ServerPort { get; set; } = 0;
        public int ShutDownPort { get; set; } = 0;
        public string ServerName { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"{{{nameof(Instance)}={Instance}, {nameof(ServerPort)}={ServerPort.ToString()}, {nameof(ShutDownPort)}={ShutDownPort.ToString()}, {nameof(ServerName)}={ServerName}}}";
        }
    }
}
