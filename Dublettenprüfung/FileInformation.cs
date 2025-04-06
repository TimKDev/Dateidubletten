namespace Dublettenprüfung;

public class FileInformation
{
    public string Name { get; }
    public long Size { get; }
    public bool IsDirectory { get; }

    private FileInformation(string name, long size, bool isDirectory)
    {
        Name = name;
        Size = size;
        IsDirectory = isDirectory;
    }

    public static FileInformation CreateFile(string name, long size)
    {
        return new FileInformation(name, size, false);
    }

    public static FileInformation CreateDirectory(string name)
    {
        return new FileInformation(name, -1, true);
    }
}