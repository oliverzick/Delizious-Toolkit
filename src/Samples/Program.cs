[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Delizious
{
    using System;
    using System.Linq;
    
    internal class Program
    {
        static void Main(string[] args)
        {
            MatchSamples();
        }

        private static void MatchSamples()
        {
            // Match when value <= -1 or value == 2 or (value => 4 and value < 7) or value > 10
            var match = Match.Any(Match.LessThanOrEqualTo(-1),
                                  Match.EqualTo(2),
                                  Match.All(Match.GreaterThanOrEqualTo(4),
                                            Match.LessThan(7)),
                                  Match.GreaterThan(10));

            var values = Enumerable.Range(-5, 20);

            Console.WriteLine("Match when value <= -1 or value == 2 or (value => 4 and value < 7) or value > 10");

            foreach (var value in values)
            {
                var result = match.Matches(value);

                Console.WriteLine($"Match value {value}: {result}");
            }
        }
    }
}
