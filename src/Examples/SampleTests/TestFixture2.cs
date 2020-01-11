using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleTests
{
    [TestFixture]
    public class TestFixture2
    {
        [Test]
        public async Task Test2A()
        {
            Console.WriteLine("Test 2A started");

            await Task.Delay(2000);

            Assert.Pass("Test 2A passed");
        }
        
        [Test]
        public async Task Test2B()
        {
            Console.WriteLine("Test 2B started");

            await Task.Delay(2000);

            Assert.Pass("Test 2B passed");
        }
        
        [Test]
        public async Task Test2C()
        {
            Console.WriteLine("Test 2C started");

            await Task.Delay(2000);

            Assert.Pass("Test 2C passed");
        }
        
        [Test]
        public async Task Test2D()
        {
            Console.WriteLine("Test 2D started");

            await Task.Delay(2000);

            Assert.Pass("Test 2D passed");
        }
        
        [Test]
        public async Task Test2E()
        {
            Console.WriteLine("Test 2E started");

            await Task.Delay(2000);

            Assert.Pass("Test 2E passed");
        }
    }
}
