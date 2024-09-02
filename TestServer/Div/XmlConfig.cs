using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestServer.Div
{
    public class XmlConfig
    {
        public void Read(string filename)
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(filename);

            XmlNode portNode = //portNode is the name of the node in this C# file, placeholder name would be xxNode
                configDoc.DocumentElement.SelectSingleNode("<ServerPort");
            if (portNode != null ) //Standard for an int
            {
                string portStr = portNode.InnerText.Trim();
                int port = Convert.ToInt32(portStr);
            }

            XmlNode nameNode =
                configDoc.DocumentElement.SelectSingleNode("<ServerName>");
            if (nameNode != null ) //Standard for a str
            {
                string name = nameNode.InnerText.Trim();
            }
        }
    }
}
