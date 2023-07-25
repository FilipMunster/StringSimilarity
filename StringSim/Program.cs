using F23.StringSimilarity;
using StringSim;
using System.Runtime.CompilerServices;

string[] db = File.ReadAllLines("db.txt");
List<(string Word, string Correct)> test = File.ReadAllLines("test.txt")
    .Select(x =>
    {
        var splitted = x.Split(';');
        return (splitted[0], splitted[1]);
    })
    .ToList();
ModelComparer comparer = new(db);

List<TestResult> results = new List<TestResult>();

foreach (var item in test)
{
    List<(string ModelName, bool Passed)> testResult = comparer.Test(item.Word, item.Correct);
    foreach (var result in testResult)
    {
        if (!results.Any(x => x.ModelName == result.ModelName))
            results.Add(new TestResult() { ModelName = result.ModelName });

        results.First(x => x.ModelName == result.ModelName).Results.Add((item.Word, result.Passed));
    }
}

foreach (var result in results)
{
    Console.WriteLine($"{result.ModelName}: {String.Format("{0:N2}", result.SuccesPercentage)} % ({result.PassedCount}/{result.Results.Count})");
    result.WriteNotPassed();
    Console.WriteLine("-----------------");
}
Console.ReadKey();