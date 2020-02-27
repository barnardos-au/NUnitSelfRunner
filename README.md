# NUnitSelfRunner
A shim to turn a class library containing NUnit fixtures into a console app which can self-run tests

## Introduction
NUnitSelfRunner is a NET Standard 2.0 class library which when added to an [NUnit](https://nunit.org/) test project and making a few small changes, turns it into a self-executing test runner.

The main reason why you'd want to do this is if you're automating your tests runs through Continuous Integration systems. Before we go into the specifics of the library, first let's have a look at your options for running NUnit tests.

### 1. Through an IDE

Visual Studio provides a built-in test [explorer](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019) which, for NET Framework projects, can run your NUnit test fixtures out-of-the-box. If your test projects target NET Core then you'll need to add some nuget packages including [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/) and [NUnit3TestAdapter](https://www.nuget.org/packages/NUnit3TestAdapter/), but the experience is the same.

As an alternative, if you're using [Resharper](https://www.jetbrains.com/resharper/) then you'll have access to its more comprehensive test runner. The above requirements are still the same though.

If you're lucky enough to be using [Rider](https://www.jetbrains.com/rider/) IDE then again you'll have access to the same Resharper test runner.

### 2. From the command line

For NET Core projects, Microsoft provides the `dotnet` CLI tool and in particular `dotnet test` to run all tests in your project. See [docs](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=netcore21) and NUnit testing [article](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit) for further information about CLI testing.

For NET Framework projects, NUnit provides [NUnit Console Runner](https://github.com/nunit/docs/wiki/Console-Runner) which is a separate executable which can be downloaded from [here](https://github.com/nunit/nunit-console/releases). While NUnit Console Runner appears to be the most comprehensive option for CI, it currently only supports NET Framework test projects.

### 3. Continuous Integration Systems

CI systems such as [TeamCity](https://www.jetbrains.com/teamcity/) have their own NUnit test runners, some leverage NUnit Console whilst others build on the separate [NUnit Engine](https://github.com/nunit/docs/wiki/Test-Engine-API) and API.

#### TeamCity Integration

TeamCity has several [approaches](https://www.jetbrains.com/help/teamcity/nunit-support.html) to running tests including the [Command Line](https://www.jetbrains.com/help/teamcity/getting-started-with-nunit.html#GettingStartedwithNUnit-Case1.CommandLine) option which uses NUnit Console in conjunction with the `--teamcity` argument which outputs test progress information in TeamCity [Service Messages](https://www.jetbrains.com/help/teamcity/build-script-interaction-with-teamcity.html#BuildScriptInteractionwithTeamCity-ServiceMessages) format so it can be tracked by TeamCity in real-time.

## Problem?

The above illustrates the many options for running NUnit tests. So what's missing?

1. Running a combination of NET Core and NET Framework test projects through NUnit console.
2. Custom formatting of the test output to suit various CI systems.
3. Distributed test runs: The ability to split very complex testing workloads and co-ordinate overall test progress

## Introducing NUnitSelfRunner

NUnitSelfRunner can be added to any .NET test fixtures project to turn it into a console application which can run tests via the CLI. It allows the developer to pass a custom `TextWriter` so that test progress output can be formatted or logged to a file. It provides OOTB support for TeamCity message format, and can also publish its output to a [Redis](https://redis.io/) channel to support distributed test runs.

### Getting Started

First, create a suitable test project and add some tests or see [SampleTests](https://github.com/barnardos-au/NUnitSelfRunner/tree/master/src/Examples/SampleTests) project. We'll use this project from this point forward.

### Add nuget package

Using your nuget package manager, run the following:

```
Install-Package NUnitSelfRunner
```

### Make your test project a console application

If your project is currently a class library project then you'll need to add a class and make a small change to the `.csproj` file.

Add the following start-up class:

``` csharp
public static class Program
{
    public static void Main(string[] args)
    {
        NUnitSelfRunner.Tests.Run(args);
    }
}
```

Add the following `OutputType` and `StartupObject` to your test project `.csproj` file:

``` xml
<PropertyGroup>
    ..
    <OutputType>Exe</OutputType>
    <StartupObject>SampleTests.Program</StartupObject>
</PropertyGroup>
```

And that's it. When you run your test project, it will call `Tests.Run` and pass any CLI arguments through.

### Run tests

Build your project and navigate to your project's `bin` folder. From there run: `SampleTests.exe`

```
C:\code\barnardos-au\NUnitSelfRunner\src\Examples\SampleTests\bin\Debug\netcoreapp3.0>SampleTests.exe
<start-run count='0'/><start-suite id="0-1024" parentId="" name="SampleTests.dll" fullname="C:/code/barnardos-au/NUnitSelfRunner/src/Examples/SampleTests/bin/Debug/netcoreapp3.0/SampleTests.dll" type="Assembly"/><start-suite id="0-1025" parentId="0-1024" name="SampleTests" fullname="SampleTests" type="TestSuite"/><start-suite id="0-1000" parentId="0-1025" name="TestFixture1" fullname="SampleTests.TestFixture1" type="TestFixture"/><start-suite id="0-1006" parentId="0-1025" name="TestFixture2" fullname="SampleTests.TestFixture2" type="TestFixture"/><start-suite id="0-1012" parentId="0-1025" name="TestFixture3" fullname="SampleTests.TestFixture3" type="TestFixture"/><start-suite id="0-1018" parentId="0-1025" name="TestFixture4" fullname="SampleTests.TestFixture4" type="TestFixture"/><start-test id="0-1001" parentId="0-1000" name="Test1A" fullname="SampleTests.TestFixture1.Test1A" type="TestMethod"/><start-test id="0-1007" parentId="0-1006" 
...
...
...
```

You should see all 20 tests run.

### CLI arguments

Type `SampleTests.exe --help` to see possible CLI arguments.

```
PS C:\code\barnardos-au\NUnitSelfRunner\src\Examples\SampleTests\bin\Debug\netcoreapp3.0> .\SampleTests.exe --help      SampleTests 1.0.0
Copyright (C) 2020 SampleTests

  -e, --explore     (Default: false) Display test info

  -c, --console     (Default: false) Display concise console output

  -f, --filter      Test filter selection
  
  -l, --testlist    The name (or path) of a file containing a list of tests to run or explore, one per line

  -s, --settings    Settings

  -t, --teamcity    (Default: false) Use TeamCity event listener

  -o, --outputfile  Name of file to output console
  
  -r, --redis       Redis Host

  -q, --queue       (Default: test-logs) Queue Name

  --help            Display this help screen.

  --version         Display version information.
```

The following is a description of the CLI arguments

| Arg (short) | Arg (long) | Description |
| ----------- | ----------- | ----------- |
| -e | --explore | Explore (display) tests. See --explore argument in [NUnit docs](https://github.com/nunit/docs/wiki/Console-Command-Line) |
| -c | --console | Simple output format which displays a line for each test showing test name and its result: Passed, Failed, Ignored etc. |
| -f | --filter | Allows filtering tests to run or explore. See --where argument in [NUnit docs](https://github.com/nunit/docs/wiki/Console-Command-Line) |
| -l | --testlist | The name (or path) of a file containing a list of tests to run or explore, one per line |
| -s | --settings | Adds settings as key/value pairs to NUnit test engine. Can be repeated. Example: `-s NumberOfTestWorkers=4` |
| -t | --teamcity | Outputs in TeamCity Service Message format |
| -o | --output | Name of file to output console |
| -r | --redis | Sends output to Redis queue (channel). Specify redis host connection. Example: `-r localhost:6379` |
| -q | --queue | Used in conjunction with `-r` to specify the queue (channel) name. Default `test-logs` |

## Distributed testing

One scenario this library can be used for is distributed test runs. When a project has a massive amount of tests to run, executing tests on a single host may take too long. It is possible to make use of parallelisation to run multiple tests in parallel and using TeamCity or other CI system, a pipeline can split large jobs across multiple workers, but sometimes this is not enough.

Another way is to create a distributed workflow and utilise multiple (or lots) of test runners where each runner will be given a set of tests to run. Each test runner must report its test run state back to a host which in turn will report back to the CI system.

One way of achieving this is to use the pub/sub feature of Redis. An *orchestrator* app (can be a simple console app) will split tests into batches.
Each batch will be given to a *worker* which will run those tests. The *worker* will output its results to Redis by *publishing* to the Redis `test-log` channel. The *orchestrator* app will *subscribe* to the `test-log` channel. It will receive test result output from all *workers*. It will output received messages to the console. The orchestrator will be run from the CI system meaning that all log output (including test results) will be captured by the CI system.

### Quick test

Install redis using Docker. Type `docker run -p 6379:6379 -d --name my-redis redis`. Run [RedisChannelSubscriber](https://github.com/barnardos-au/NUnitSelfRunner/tree/master/src/Examples/RedisChannelSubscriber) sample app. When launched it will subscribe to the `test-logs` channel and display all output to the console.

Now return to your sample tests project. Type: `SampleTests.exe -c -r localhost:6379`. Note that all output from the test run will be captured and displayed in the `RedisChannelSubscriber` app.

## Custom output format

It is possible to implement a custom `TextWriter` and pass it through to the Test Runner using `NUnitSelfRunner.Tests.Run(args, customTextWriter)`. This will allow, for instance, writing output to a file.

