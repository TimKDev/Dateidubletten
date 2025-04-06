using Dublettenprüfung.Public;

namespace TestApplication;

class Program
{
    static void Main(string[] args)
    {
        var dublettenPrüfung = Dublettenprüfung.Public.Dublettenprüfung.Create();
        
        string testPath;
        if (args.Length > 0)
        {
            testPath = args[0];
        }
        else
        {
            Console.WriteLine("Please enter a directory path:");
            testPath = Console.ReadLine() ?? "/";
        }
        
        Console.WriteLine($"Scanning directory: {testPath}");
        var result = dublettenPrüfung.Sammle_Kandidaten(testPath, Vergleichsmodi.Größe).ToList();
        Console.WriteLine($"Found {result.Count} potential duplicates");
        
        var result2 = dublettenPrüfung.Prüfe_Kandidaten(result).ToList();
        Console.WriteLine($"Verified {result2.Count} actual duplicates");
        
        // Display results
        foreach (var dublette in result2)
        {
            Console.WriteLine("\nDuplicate files:");
            foreach (var path in dublette.Dateipfade)
            {
                Console.WriteLine($"  {path}");
            }
        }
        
        Console.WriteLine("Program finished");
    }
}