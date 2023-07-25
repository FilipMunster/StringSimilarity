
using F23.StringSimilarity;
using F23.StringSimilarity.Experimental;
using F23.StringSimilarity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSim
{
    internal class ModelComparer
    {
        private readonly NormalizedLevenshtein normalizedLevenshtein = new NormalizedLevenshtein();
        private readonly WeightedLevenshtein weightedLevenshtein = new WeightedLevenshtein(new CharSubstitution());
        private readonly Damerau damerau = new Damerau();
        private readonly JaroWinkler jaroWinkler = new JaroWinkler();
        private readonly LongestCommonSubsequence longestCommonSubsequence = new LongestCommonSubsequence();
        private readonly MetricLCS metricLCS = new MetricLCS();
        private readonly NGram nGram = new NGram(2);
        private readonly QGram qGram = new QGram(2);
        private readonly Cosine cosine = new Cosine(2);

        private IEnumerable<string> _Db { get; }
        private List<(string ModelName, IStringDistance Model)> _Models { get; set; }
        public ModelComparer(IEnumerable<string> db)
        {
            _Db = db;
            _Models = new()
            {
                (nameof(normalizedLevenshtein), normalizedLevenshtein),
                (nameof(weightedLevenshtein), weightedLevenshtein),
                (nameof(damerau), damerau),
                (nameof(jaroWinkler), jaroWinkler),
                (nameof(longestCommonSubsequence), longestCommonSubsequence),
                (nameof(metricLCS), metricLCS),
                (nameof(nGram), nGram),
                (nameof(qGram), qGram),
                (nameof(cosine), cosine)
            };
            
        }

        public void Compare(string word, int count)
        {
            foreach (var model in _Models)
            {
                var closestWords = _FindClosest(model.Model, word, count);
                Console.WriteLine("------------------------------------");
                Console.WriteLine(model.ModelName + ":");
                foreach (var closest in closestWords)
                {
                    Console.WriteLine(closest.Word + " (" + closest.Distance + ")");
                }
            }
            Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||");
        }

        public List<(string ModelName, bool Passed)> Test(string word, string correct)
        {
            List<(string ModelName, bool Passed)> result = new();
            foreach (var model in _Models)
            {
                var closestWords = _FindClosest(model.Model, word, 1);
                result.Add((model.ModelName, closestWords[0].Word == correct));
            }
            return result;
        }

        private List<(string Word, double Distance)> _FindClosest(IStringDistance model, string word, int count)
        {
            List<(string Word, double Distance)> result = new();
            foreach (var item in _Db)
            {
                result.Add((item, model.Distance(word, item)));
            }
            return result.OrderBy(x => x.Distance)
                .Take(count)
                .ToList();
        }


        private class CharSubstitution : ICharacterSubstitution
        {
            public double Cost(char c1, char c2)
            {
                if (c1 == '-' && c2 == '_') return 0.1;

                return 1.0;
            }
        }
    }
}
