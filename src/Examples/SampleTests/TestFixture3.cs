using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleTests
{
    [TestFixture]
    public class TestFixture3
    {
        [Test]
        public async Task Test3A()
        {
            Console.WriteLine("Test 3A started");

            await Task.Delay(2000);

            Assert.Pass("Test 3A passed");
        }
        
        [Test]
        public async Task Test3B()
        {
            Console.WriteLine("Test 3B started");

            await Task.Delay(2000);

            Assert.Pass("Test 3B passed");
        }
        
        [Test]
        public async Task Test3C()
        {
            Console.WriteLine("Test 3C started");

            await Task.Delay(2000);

            Assert.Pass("Test 3C passed");
        }
        
        [Test]
        public async Task Test3D()
        {
            Console.WriteLine("Test 3D started");

            await Task.Delay(2000);

            Assert.Pass("Test 3D passed");
        }
        
        [Test]
        public async Task Test3E()
        {
            Console.WriteLine("Test 3E started");

            await Task.Delay(2000);

            Assert.Pass("Test 3E passed");
        }
    }
}
