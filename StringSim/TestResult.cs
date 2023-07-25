using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSim
{
    internal class TestResult
    {
        public string ModelName { get; set; }
        public List<(string Word, bool Passed)> Results { get; set; } = new();
        public int NotPassedCount => Results.Where(x => !x.Passed).Count();
        public int PassedCount => Results.Where(x => x.Passed).Count();
        public double SuccesPercentage => 100.0 * PassedCount / Results.Count;

        public void WriteNotPassed()
        {
            var mistakes = Results.Where(x => !x.Passed);
            foreach (var mistake in mistakes)
            {
                Console.WriteLine($"Not passed: {mistake.Word}");
            }
        }

    }
}
