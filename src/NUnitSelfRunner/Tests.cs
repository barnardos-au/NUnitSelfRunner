using System;
using System.IO;
using System.Reflection;
using CommandLine;
using NUnit.Engine;
using NUnit.Engine.Listeners;
using NUnitSelfRunner.Listeners;
using StackExchange.Redis;

namespace NUnitSelfRunner
{
    public class Tests
    {
        private TextWriter textWriter;
        private readonly Assembly testFixtureAssembly;

        public Tests(TextWriter textWriter, Assembly testFixtureAssembly)
        {
            this.textWriter = textWriter;

            this.testFixtureAssembly = testFixtureAssembly ?? Assembly.GetEntryAssembly();
        }
        
        public static void Run(string[] args, Assembly testFixtureAssembly = null, TextWriter textWriter = null)
        {
            var tests = new Tests(textWriter, testFixtureAssembly);

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(tests.Start);
        }

        private void Start(Options options)
        {
            if (textWriter == null)
                textWriter = Console.Out;

            var testPackage = new TestPackage(testFixtureAssembly.Location);
            foreach (var setting in options.GetSettings())
            {
                testPackage.AddSetting(setting.Key, setting.Value);
            }
            
            var testEngine = TestEngineActivator.CreateInstance();
            var filterService = testEngine.Services.GetService<ITestFilterService>();
            var testFilterBuilder = filterService.GetTestFilterBuilder();

            if (!string.IsNullOrEmpty(options.TestListFile))
            {
                var fileInfo = new FileInfo(options.TestListFile);
                if (!fileInfo.Exists) throw new ApplicationException($"File: {options.TestListFile} not found");
                
                var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var test = streamReader.ReadLine();
                        testFilterBuilder.AddTest(test);
                    }
                }
            }

            if (!string.IsNullOrEmpty(options.Filter))
            {
                testFilterBuilder.SelectWhere(options.Filter);
            }

            var testFilter = testFilterBuilder.GetFilter();

            using (var testRunner = testEngine.GetRunner(testPackage))
            {
                var testEventListener = GetTestEventListener(options);
                
                if (options.Explore)
                {
                    var result = testRunner.Explore(testFilter);
                    var xml = result.OuterXml;
                    testEventListener.OnTestEvent(xml);

                    if (string.IsNullOrEmpty(options.OutputFile)) return;
                    
                    using (var fileWriter = new StreamWriter(options.OutputFile))
                    {
                        fileWriter.Write(xml);
                    }
                }
                else
                {
                    var result = testRunner.Run(testEventListener, testFilter);
                    var xml = result.OuterXml;
                    if (string.IsNullOrEmpty(options.OutputFile)) return;
                    
                    using (var fileWriter = new StreamWriter(options.OutputFile))
                    {
                        fileWriter.Write(xml);
                    }
                }
            }
        }

        private ITestEventListener GetTestEventListener(Options options)
        {
            if (!string.IsNullOrEmpty(options.RedisHost))
            {
                var redis = ConnectionMultiplexer.Connect(options.RedisHost);
                var subscriber = redis.GetSubscriber();

                textWriter = new RedisQueueWriter(subscriber, options.QueueName);
            }

            ITestEventListener testEventListener = new DefaultEventListener(textWriter);

            if (options.Explore)
            {
                return testEventListener;
            }

            if (options.Console)
            {
                return new ConsoleEventListener(textWriter);
            }
            
            if (options.TeamCity)
            {
                return new TeamCityEventListener(textWriter, new TeamCityInfo());
            }

            return testEventListener;
        }
    }
}
