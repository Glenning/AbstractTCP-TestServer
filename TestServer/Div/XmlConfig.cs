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

            XmlNode portNode = configDoc.DocumentElement.SelectSingleNode("port"); //portNode is the name of the node in this C# file, placeholder name would be xxNode
            if (portNode != null) //Standard for an int
            {
                String portStr = portNode.InnerText.Trim();
                int port = Convert.ToInt32(portStr);
            }

            XmlNode nameNode = configDoc.DocumentElement.SelectSingleNode ("name");
            if (nameNode != null) //Standard for a str
            {
                String name = nameNode.InnerText.Trim();
            }
        }
    }
}
