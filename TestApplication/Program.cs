using Dublettenprüfung.Public;

namespace TestApplication;

class Program
{
    static void Main(string[] args)
    {
        var dublettenPrüfung = Dublettenprüfung.Public.Dublettenprüfung.Create();
        var testPath = "/home/tim/Source/";
        var result = dublettenPrüfung.Sammle_Kandidaten(testPath, Vergleichsmodi.Größe).ToList();
        var result2 = dublettenPrüfung.Prüfe_Kandidaten(result).ToList();
        Console.WriteLine("Program finished");
    }
}