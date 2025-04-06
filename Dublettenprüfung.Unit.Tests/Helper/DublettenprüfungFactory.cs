using Dublettenprüfung.Public;

namespace Dublettenprüfung.Unit.Tests.Helper;

internal static class DublettenprüfungFactory
{
    internal static IDublettenprüfung Create(IFileRepository fileRepository)
    {
        return new Public.Dublettenprüfung(fileRepository);
    }
}