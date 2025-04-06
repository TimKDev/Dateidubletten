using Dublettenprüfung.Public;

namespace Dublettenprüfung;

internal class GroupedFiles
{
    private readonly IEqualityComparer<FileInformation> _fileInfoComparer;
    private readonly string _basePath;
    private Dictionary<FileInformation, List<string>> Result { get; }

    public GroupedFiles(string basePath, IEqualityComparer<FileInformation> fileInfoComparer)
    {
        _fileInfoComparer = fileInfoComparer;
        _basePath = basePath;
        Result = new Dictionary<FileInformation, List<string>>(fileInfoComparer);
    }

    public IEnumerable<IDublette> GetDublettes()
    {
        return Result.Values.Where(files => files.Count > 1)
            .Select(groupedFiles => new Dublette(groupedFiles.Select(ExtendByBasePath)));
    }

    public IEnumerable<IGrouping<FileInformation, FileInformation>> GroupByComparer(List<FileInformation> fileInfos)
    {
        return fileInfos.GroupBy(file => file, _fileInfoComparer);
    }

    public void AddItemByGroupingKey(IGrouping<FileInformation, FileInformation> groupedFile)
    {
        if (Result.TryGetValue(groupedFile.Key, out var dubletteEntry))
        {
            dubletteEntry.AddRange(groupedFile.Select(fileInfo => fileInfo.Name));
            return;
        }

        Result[groupedFile.Key] = groupedFile.Select(fileInfo => fileInfo.Name).ToList();
    }

    public void Combine(GroupedFiles groupedFiles)
    {
        foreach (var itemToAdd in groupedFiles.Result)
        {
            var paths = itemToAdd.Value.Select(groupedFiles.ExtendByBasePath).ToList();
            if (Result.TryGetValue(itemToAdd.Key, out var dublettEntry))
            {
                dublettEntry.AddRange(paths);
            }
            else
            {
                Result.Add(itemToAdd.Key, paths);
            }
        }
    }

    private string ExtendByBasePath(string fileName)
    {
        return Path.Combine(_basePath, fileName);
    }
}