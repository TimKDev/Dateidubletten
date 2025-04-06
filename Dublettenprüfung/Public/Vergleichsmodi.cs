namespace Dublettenprüfung.Public;

/// <summary>
/// Defines the different modi for file comparison. 
/// </summary>
public enum Vergleichsmodi
{
    /// <summary>
    /// Modus in which two files are considered equal when their names and sizes are equal.
    /// </summary>
    Größe_und_Name,

    /// <summary>
    /// Modus in which two files are considered equal when their sizes are equal. 
    /// </summary>
    Größe
}