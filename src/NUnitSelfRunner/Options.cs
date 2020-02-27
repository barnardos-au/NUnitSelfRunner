using System.Collections.Generic;
using CommandLine;

namespace NUnitSelfRunner
{
    internal class Options
    {
        [Option('e', "explore",  Default = false,  HelpText = "Display test info")]
        public bool Explore { get; set; }

        [Option('c', "console",  Default = false,  HelpText = "Display concise console output")]
        public bool Console { get; set; }

        [Option('f', "filter", Required = false, HelpText = "Test filter selection")]
        public string Filter { get; set; }

        [Option('l', "testlist", Required = false, HelpText = "The name (or path) of a file containing a list of tests to run or explore, one per line")]
        public string TestListFile { get; set; }

        [Option('s', "settings", Required = false, HelpText = "Settings")]
        public IEnumerable<string> SettingArgs { get; set; }

        [Option('t', "teamcity",  Default = false,  HelpText = "Use TeamCity event listener")]
        public bool TeamCity { get; set; }

        [Option('o', "outputfile", Required = false, HelpText = "Name of file to output console")]
        public string OutputFile { get; set; }

        [Option('r', "redis", Required = false, HelpText = "Redis Host")]
        public string RedisHost { get; set; }

        [Option('q', "queue", Required = false, HelpText = "Queue Name", Default = "test-logs")]
        public string QueueName { get; set; }

        public Dictionary<string, object> GetSettings()
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var arg in SettingArgs)
            {
                var parts = arg.Split('=');
                var left = parts[0];
                var right = parts.Length > 1 ? parts[1] : string.Empty;
                if (int.TryParse(right, out var num))
                {
                    dictionary[left] = num;
                }
                else
                {
                    dictionary[left] = right;
                }
            }

            return dictionary;
        }
    }
}