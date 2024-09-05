using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer.Div
{
    public class Logger
    {
        private const string logname = "MyLogger";

        public Logger() { }

        public void Start()
        {
            TraceSource ts = new TraceSource(logname);
            ts.Switch = new SourceSwitch(logname, SourceLevels.Information.ToString());

            ts.Listeners.Add(new ConsoleTraceListener());

            ts.Listeners.Add(
                new TextWriterTraceListener($"{logname}.txt")
                { Filter = new EventTypeFilter(SourceLevels.Error) });

            ts.Listeners.Add(new XmlWriterTraceListener($"{logname}.xml")); //add a root to this

            ts.TraceEvent(TraceEventType.Information, 700, "Information");
            ts.TraceEvent(TraceEventType.Warning, 700, "Warning");
            ts.TraceEvent(TraceEventType.Error, 700, "Error");
            ts.TraceEvent(TraceEventType.Critical, 700, "Critical");
            ts.Close();

        }
    }
}
