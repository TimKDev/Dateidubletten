namespace Dublettenprüfung;

internal class FileGrouper
{
    private readonly IEqualityComparer<FileInformation> _fileInfoComparer;
    private readonly IFileRepository _fileRepository;

    public FileGrouper(IEqualityComparer<FileInformation> fileInfoComparer, IFileRepository fileRepository)
    {
        _fileInfoComparer = fileInfoComparer;
        _fileRepository = fileRepository;
    }

    public GroupedFiles GroupFiles(string path)
    {
        var foundDublettes = new GroupedFiles(path, _fileInfoComparer);
        var fileInfos = _fileRepository.GetFilesOfDirectory(path);
        var groupedFiles = foundDublettes.GroupByComparer(fileInfos);

        foreach (var groupedFile in groupedFiles)
        {
            if (groupedFile.Key.IsDirectory)
            {
                var groupedFilesFromSubFolder = GroupFiles(Path.Combine(path, groupedFile.Key.Name));

                foundDublettes.Combine(groupedFilesFromSubFolder);
                continue;
            }

            foundDublettes.AddItemByGroupingKey(groupedFile);
        }

        return foundDublettes;
    }
}