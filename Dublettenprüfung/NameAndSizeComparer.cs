namespace Dublettenprüfung;

internal class NameAndSizeComparer : IEqualityComparer<FileInformation>
{
    public bool Equals(FileInformation? x, FileInformation? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.IsDirectory || y.IsDirectory)
        {
            return false;
        }

        return string.Equals(x.Name, y.Name, StringComparison.Ordinal) && x.Size == y.Size;
    }

    public int GetHashCode(FileInformation obj)
    {
        return HashCode.Combine(
            obj.Name.GetHashCode(StringComparison.Ordinal),
            obj.Size);
    }
}