namespace Dublettenprüfung;

internal class SizeComparer : IEqualityComparer<FileInformation>
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

        return x.Size == y.Size;
    }

    public int GetHashCode(FileInformation obj)
    {
        return obj.Size.GetHashCode();
    }
}