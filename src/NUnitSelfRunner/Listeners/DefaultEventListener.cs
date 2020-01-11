using System;
using System.IO;
using NUnit.Engine;

namespace NUnitSelfRunner.Listeners
{
    public class DefaultEventListener : ITestEventListener
    {
        private readonly TextWriter outWriter;
        public DefaultEventListener() : this(Console.Out) { }

        public DefaultEventListener(TextWriter outWriter)
        {
            this.outWriter = outWriter ?? throw new ArgumentNullException("outWriter");
        }

        public void OnTestEvent(string report)
        {
            outWriter.Write(report);
        }
    }
}