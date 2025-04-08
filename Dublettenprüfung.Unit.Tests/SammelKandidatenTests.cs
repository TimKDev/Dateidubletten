using Dublettenprüfung.Public;
using Dublettenprüfung.Unit.Tests.Helper;
using FluentAssertions;
using NSubstitute;
using TE = Dublettenprüfung.Unit.Tests.Helper.TestEntities;

namespace Dublettenprüfung.Unit.Tests;

public class SammelKandidatenTests
{
    private readonly IFileRepository _fileRepositoryMock = Substitute.For<IFileRepository>();
    private const string BasePath = "/";

    [Fact]
    public void ShouldReturnNoDublettes_GivenFolderWithNoItems()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        _fileRepositoryMock.GetFilesOfDirectory(BasePath).Returns([]);

        var res = sut.Sammle_Kandidaten(BasePath);

        res.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnOne_GivenDefaultModus()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        SetupMockWithOneSubFolder();

        var res = sut.Sammle_Kandidaten(BasePath).ToList();

        res.Should().HaveCount(1);
        res.Single().Dateipfade.Should().BeEquivalentTo([
            CombineWithBase(TE.Name1), CombineWithBase(Path.Combine(TE.DirName1, TE.Name1))
        ]);
    }


    [Fact]
    public void ShouldReturnOne_GivenSizeOnlyModus()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        SetupMockWithOneSubFolder();

        var res = sut.Sammle_Kandidaten(BasePath, Vergleichsmodi.Größe).ToList();

        res.Should().HaveCount(1);
        res.Single().Dateipfade.Should().BeEquivalentTo([
            CombineWithBase(TE.Name1), CombineWithBase(Path.Combine(TE.DirName1, TE.Name1)), CombineWithBase(TE.Name3)
        ]);
    }


    [Fact]
    public void ShouldReturnTwo_GivenDefaultModus()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        SetupComplexThreeFolderMock();

        var res = sut.Sammle_Kandidaten(BasePath).ToList();

        res.Should().HaveCount(2);
        List<List<string>> combinedResults = [res[0].Dateipfade.ToList(), res[1].Dateipfade.ToList()];
        combinedResults.Should().BeEquivalentTo(new List<List<string>>
            {
                new()
                {
                    CombineWithBase(TE.Name1), CombineWithBase(Path.Combine(TE.DirName1, TE.Name1)),
                    CombineWithBase(Path.Combine(TE.DirName2, TE.Name1)),
                    CombineWithBase(Path.Combine(TE.DirName1, TE.DirName3, TE.Name1))
                },
                new()
                {
                    CombineWithBase(TE.Name2), CombineWithBase(Path.Combine(TE.DirName1, TE.Name2)),
                    CombineWithBase(Path.Combine(TE.DirName1, TE.DirName3, TE.Name2))
                },
            }
        );
    }

    [Fact]
    public void ShouldReturnTwo_GivenSizeOnlyModus()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        SetupComplexThreeFolderMock();

        var res = sut.Sammle_Kandidaten(BasePath, Vergleichsmodi.Größe).ToList();

        res.Should().HaveCount(1);

        res.Single().Dateipfade.Should().BeEquivalentTo([
            CombineWithBase(TE.Name1), CombineWithBase(Path.Combine(TE.DirName1, TE.Name1)),
            CombineWithBase(Path.Combine(TE.DirName2, TE.Name1)),
            CombineWithBase(Path.Combine(TE.DirName1, TE.DirName3, TE.Name1)),
            CombineWithBase(TE.Name2), CombineWithBase(Path.Combine(TE.DirName1, TE.Name2)),
            CombineWithBase(Path.Combine(TE.DirName1, TE.DirName3, TE.Name2))
        ]);
    }

    [Fact]
    public void ShouldThrowInvalidArgumentException_GivenNullAsInput()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);

        var res = () => sut.Sammle_Kandidaten(null!).ToList();

        res.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowInvalidArgumentException_GivenEmptyStringAsInput()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);

        var res = () => sut.Sammle_Kandidaten("   ").ToList();

        res.Should().Throw<ArgumentException>();
    }

    private void SetupComplexThreeFolderMock()
    {
        _fileRepositoryMock.GetFilesOfDirectory(BasePath)
            .Returns([
                Dir(TE.DirName1), Dir(TE.DirName2), File(TE.Name1, TE.Size1), File(TE.Name3, TE.Size3),
                File(TE.Name2, TE.Size1)
            ]);

        _fileRepositoryMock.GetFilesOfDirectory(CombineWithBase(TE.DirName1)).Returns([
            Dir(TE.DirName3), File(TE.Name1, TE.Size1), File(TE.Name4, TE.Size4), File(TE.Name2, TE.Size1),
            File(TE.Name5, TE.Size5)
        ]);

        _fileRepositoryMock.GetFilesOfDirectory(CombineWithBase(TE.DirName2)).Returns([
            File(TE.Name1, TE.Size1), File(TE.Name6, TE.Size6), File(TE.Name7, TE.Size7)
        ]);

        _fileRepositoryMock.GetFilesOfDirectory(CombineWithBase(Path.Combine(TE.DirName1, TE.DirName3))).Returns([
            File(TE.Name1, TE.Size1), File(TE.Name8, TE.Size8), File(TE.Name2, TE.Size1), File(TE.Name9, TE.Size9)
        ]);
    }

    private void SetupMockWithOneSubFolder()
    {
        _fileRepositoryMock.GetFilesOfDirectory(BasePath)
            .Returns([
                Dir(TE.DirName1), File(TE.Name1, TE.Size1), File(TE.Name2, TE.Size2), File(TE.Name3, TE.Size1)
            ]);
        _fileRepositoryMock.GetFilesOfDirectory(CombineWithBase(TE.DirName1)).Returns([File(TE.Name1, TE.Size1)]);
    }

    private static FileInformation File(string name, long size)
    {
        return FileInformation.CreateFile(name, size);
    }


    private static FileInformation Dir(string name)
    {
        return FileInformation.CreateDirectory(name);
    }

    private string CombineWithBase(string path)
    {
        return Path.Combine(BasePath, path);
    }
}