using NBench;

namespace StressTest
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return NBench.NBenchRunner.Run<LoadControllerStressTest>();
        }
    }
}