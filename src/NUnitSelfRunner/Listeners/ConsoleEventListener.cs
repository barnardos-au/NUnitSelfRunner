using System;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Listeners;

namespace NUnitSelfRunner.Listeners
{
    public class ConsoleEventListener : ITestEventListener
    {
        private readonly TextWriter outWriter;
        public ConsoleEventListener() : this(Console.Out) { }

        public ConsoleEventListener(TextWriter outWriter)
        {
            this.outWriter = outWriter ?? throw new ArgumentNullException("outWriter");
        }

        public void OnTestEvent(string report)
        {
            var doc = new XmlDocument();
            doc.LoadXml(report);

            var testEvent = doc.FirstChild;
            RegisterMessage(testEvent);
        }
        
        public void RegisterMessage(XmlNode xmlEvent)
        {
            if (xmlEvent == null) throw new ArgumentNullException("xmlEvent");
            var messageName = xmlEvent.Name;
            if (string.IsNullOrEmpty(messageName))
            {
                return;
            }
            
            var fullName = xmlEvent.GetAttribute("fullname");
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = xmlEvent.GetAttribute("testname");
                if (string.IsNullOrEmpty(fullName))
                {
                    return;
                }
            }

            if (messageName == "test-case")
            {
                var result = xmlEvent.GetAttribute("result");
                
                outWriter.WriteLine($"{fullName}, {result}");
            }
            if (messageName == "test-run")
            {
                var result = xmlEvent.GetAttribute("result");
                var total = xmlEvent.GetAttribute("total");
                var passed = xmlEvent.GetAttribute("passed");
                var failed = xmlEvent.GetAttribute("failed");
                var inconclusive = xmlEvent.GetAttribute("inconclusive");
                var skipped = xmlEvent.GetAttribute("skipped");
                var duration = xmlEvent.GetAttribute("duration");
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine($"Result: {result}, duration: {duration}");
                sb.AppendLine(
                    $"Total:{total}, passed:{passed}, failed:{failed}, inconclusive:{inconclusive}, skipped:{skipped}");
                
                outWriter.Write(sb.ToString());
            }
        }
    }
}
