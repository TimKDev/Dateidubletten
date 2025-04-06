using Dublettenprüfung.Public;

namespace Dublettenprüfung;

internal class Dublette : IDublette
{
    public IEnumerable<string> Dateipfade { get; }

    public Dublette(IEnumerable<string> dateipfade)
    {
        Dateipfade = dateipfade;
    }
}