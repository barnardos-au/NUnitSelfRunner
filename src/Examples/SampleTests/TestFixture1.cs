using System;
using System.Threading.Tasks;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace SampleTests
{
    [TestFixture]
    public class TestFixture1
    {
        [Test]
        public async Task Test1A()
        {
            Console.WriteLine("Test 1A started");

            await Task.Delay(2000);

            Assert.Pass("Test 1A passed");
        }
        
        [Test]
        public async Task Test1B()
        {
            Console.WriteLine("Test 1B started");

            await Task.Delay(2000);

            Assert.Pass("Test 1B passed");
        }
        
        [Test]
        public async Task Test1C()
        {
            Console.WriteLine("Test 1C started");

            await Task.Delay(2000);

            Assert.Pass("Test 1C passed");
        }
        
        [Test]
        public async Task Test1D()
        {
            Console.WriteLine("Test 1D started");

            await Task.Delay(2000);

            Assert.Pass("Test 1D passed");
        }
        
        [Test]
        public async Task Test1E()
        {
            Console.WriteLine("Test 1E started");

            await Task.Delay(2000);

            Assert.Pass("Test 1E passed");
        }
    }
}
