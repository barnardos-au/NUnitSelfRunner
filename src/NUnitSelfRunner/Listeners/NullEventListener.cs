using NUnit.Engine;

namespace NUnitSelfRunner.Listeners
{
    public class NullEventListener : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
        }
    }
}
