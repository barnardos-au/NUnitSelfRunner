using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NUnitSelfRunner.Listeners
{
    public abstract class LinePrinter : TextWriter
    {
        private readonly StringBuilder stringBuilder;

        protected LinePrinter()
        {
            stringBuilder = new StringBuilder();
        }

        public override Encoding Encoding => Encoding.Default;

        public override void Write(char value)
        {
            stringBuilder.Append(value);
            var s = stringBuilder.ToString();
            if (!s.Contains(Environment.NewLine)) return;

            var lines = s.Split(CoreNewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                Print(line);
            }
            stringBuilder.Clear();
        }

        protected abstract void Print(string output);
    }
}