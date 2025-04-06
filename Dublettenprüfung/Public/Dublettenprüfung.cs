namespace Dublettenprüfung.Public;

/// <inheritdoc />
public class Dublettenprüfung : IDublettenprüfung
{
    private readonly IFileRepository _fileRepository;

    internal Dublettenprüfung(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    /// <summary>
    /// Factory Function to create a new instance of Dublettenprüfung. 
    /// </summary>
    /// <returns></returns>
    public static Dublettenprüfung Create()
    {
        return new Dublettenprüfung(new FileRepository());
    }

    /// <inheritdoc />
    public IEnumerable<IDublette> Sammle_Kandidaten(string pfad)
    {
        return Sammle_Kandidaten(pfad, Vergleichsmodi.Größe_und_Name);
    }

    /// <inheritdoc />
    public IEnumerable<IDublette> Sammle_Kandidaten(string pfad, Vergleichsmodi modus)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pfad);

        IEqualityComparer<FileInformation> fileComparerStrategy = modus switch
        {
            Vergleichsmodi.Größe_und_Name => new NameAndSizeComparer(),
            Vergleichsmodi.Größe => new SizeComparer(),
            _ => throw new NotImplementedException($"Missing Implementation for {modus}.")
        };

        var fileGrouper = new FileGrouper(fileComparerStrategy, _fileRepository);

        return fileGrouper.GroupFiles(pfad).GetDublettes();
    }

    /// <inheritdoc />
    public IEnumerable<IDublette> Prüfe_Kandidaten(IEnumerable<IDublette> kandidaten)
    {
        ArgumentNullException.ThrowIfNull(kandidaten);
        var verifiedDublettes = new List<IDublette>();

        foreach (var kandidat in kandidaten)
        {
            var hashResults = new Dictionary<string, string>();
            foreach (var filePath in kandidat.Dateipfade)
            {
                hashResults[filePath] = _fileRepository.GetMd5HashFromFile(filePath);
            }

            var groupedByHash = hashResults
                .GroupBy(pair => pair.Value)
                .Where(group => group.Count() > 1)
                .Select(group => new Dublette(group.Select(pair => pair.Key)))
                .ToList();

            verifiedDublettes.AddRange(groupedByHash);
        }

        return verifiedDublettes;
    }
}