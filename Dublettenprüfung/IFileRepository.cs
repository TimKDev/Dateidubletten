namespace Dublettenprüfung;

public interface IFileRepository
{
    List<FileInformation> GetFilesOfDirectory(string path);
    string GetMd5HashFromFile(string path);
}