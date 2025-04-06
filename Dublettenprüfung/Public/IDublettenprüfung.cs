namespace Dublettenprüfung.Public;

/// <summary>
///     Interface that exposes the functionality on how to find duplicate files in a file tree.
/// </summary>
public interface IDublettenprüfung
{
    /// <summary>
    /// This function finds all files in a file tree where the filename and size are equal.
    /// </summary>
    /// <param name="pfad">Base path where the search begins.</param>
    /// <returns>Grouping of files with equal filename and size.</returns>
    IEnumerable<IDublette> Sammle_Kandidaten(string pfad);

    /// <summary>
    /// This function finds all files in a file tree where the filename and size or only the size equal depending on
    /// the modus.
    /// </summary>
    /// <param name="pfad">Base path where the search begins.</param>
    /// <param name="modus">Defines when files are considered equal.</param>
    /// <returns>Grouping of files equal with respect to the chosen modus.</returns>
    IEnumerable<IDublette> Sammle_Kandidaten(string pfad, Vergleichsmodi modus);

    /// <summary>
    /// Returns all files with identical content from the given list of possible duplicated file groups.
    /// </summary>
    /// <param name="kandidaten">Groupings of files which should be checked for equal content</param>
    /// <returns>Groupings of files where the content is actually equal.</returns>
    IEnumerable<IDublette> Prüfe_Kandidaten(IEnumerable<IDublette> kandidaten);
}