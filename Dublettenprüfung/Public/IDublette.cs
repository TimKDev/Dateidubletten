namespace Dublettenprüfung.Public;

/// <summary>
///    Used to represent a grouping of files. 
/// </summary>
public interface IDublette
{
    /// <summary>
    /// Contains all paths of the files which are part of the grouping
    /// </summary>
    IEnumerable<string> Dateipfade { get; }
}