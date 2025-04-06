using System.Security.Cryptography;

namespace Dublettenprüfung;

internal class FileRepository : IFileRepository
{
    public List<FileInformation> GetFilesOfDirectory(string path)
    {
        var result = new List<FileInformation>();
        var directory = new DirectoryInfo(path);

        result.AddRange(
            directory.GetFiles().Select(fileInfo => FileInformation.CreateFile(fileInfo.Name, fileInfo.Length)));

        result.AddRange(
            directory.GetDirectories().Select(dirInfo => FileInformation.CreateDirectory(dirInfo.Name)));

        return result;
    }

    public string GetMd5HashFromFile(string path)
    {
        using var stream = File.OpenRead(path);
        return BitConverter.ToString(MD5.HashData(stream));
    }
}