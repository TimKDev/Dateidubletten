using Dublettenprüfung.Public;
using Dublettenprüfung.Unit.Tests.Helper;
using FluentAssertions;
using NSubstitute;
using TE = Dublettenprüfung.Unit.Tests.Helper.TestEntities;

namespace Dublettenprüfung.Unit.Tests;

public class PrüfeKandidatenTests
{
    private readonly IFileRepository _fileRepositoryMock = Substitute.For<IFileRepository>();

    [Fact]
    public void ShouldReturnEmptyResult_GivenEmptyInput()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);

        var res = sut.Prüfe_Kandidaten([]).ToList();

        res.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnOne_GivenOneDuplicatedFile()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        IEnumerable<IDublette> request = [CreateDublette([TE.Name1, TE.Name2])];
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name1).Returns(TE.Hash1);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name2).Returns(TE.Hash1);

        var res = sut.Prüfe_Kandidaten(request).ToList();

        res.Should().HaveCount(1);
        res.Single().Dateipfade.Should().BeEquivalentTo([TE.Name1, TE.Name2]);
    }

    [Fact]
    public void ShouldReturnTwo_GivenTwoRealDublettesAndOneFake()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);
        IEnumerable<IDublette> request =
        [
            CreateDublette([TE.Name1, TE.Name2]),
            CreateDublette([TE.Name3, TE.Name4, TE.Name5]),
            CreateDublette([TE.Name6, TE.Name7]),
        ];
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name1).Returns(TE.Hash1);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name2).Returns(TE.Hash1);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name3).Returns(TE.Hash2);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name4).Returns(TE.Hash2);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name5).Returns(TE.Hash3);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name6).Returns(TE.Hash4);
        _fileRepositoryMock.GetMd5HashFromFile(TE.Name7).Returns(TE.Hash5);

        var res = sut.Prüfe_Kandidaten(request).ToList();

        res.Should().HaveCount(2);
        List<List<string>> combinedResults = [res[0].Dateipfade.ToList(), res[1].Dateipfade.ToList()];
        combinedResults.Should().BeEquivalentTo(new List<List<string>>
            {
                new()
                {
                    TE.Name1, TE.Name2
                },
                new()
                {
                    TE.Name3, TE.Name4
                }
            }
        );
    }

    [Fact]
    public void ShouldThrowInvalidArgumentException_GivenNullAsInput()
    {
        var sut = DublettenprüfungFactory.Create(_fileRepositoryMock);

        var res = () => sut.Prüfe_Kandidaten(null!).ToList();

        res.Should().Throw<ArgumentException>();
    }

    private static IDublette CreateDublette(IEnumerable<string> paths)
    {
        return new Dublette(paths);
    }
}