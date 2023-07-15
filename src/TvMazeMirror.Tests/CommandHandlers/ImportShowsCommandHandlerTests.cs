using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Services;
using Xunit;

namespace TvMazeMirror.Tests.CommandHandlers;

public class ImportShowsCommandHandlerTests {
    private readonly ITvMazeClient client = Substitute.For<ITvMazeClient>();
    private readonly ITvMazeContext context = Substitute.For<ITvMazeContext>();
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ImportShowsCommandHandler handler;

    public ImportShowsCommandHandlerTests() {
        handler = new(client, context, unitOfWork);
    }

    [Fact]
    public async Task Execute_Does_Nothing_When_Rate_Limited() {
        client.GetShows(default).ReturnsForAnyArgs((IEnumerable<ShowDto>?)null);

        var result = await handler.Execute();

        result.Value.Should().BeNull();

        context.Shows.DidNotReceiveWithAnyArgs().Add(default!);
        await unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(249, 0)]
    [InlineData(250, 1)]
    [InlineData(499, 1)]
    [InlineData(500, 2)]
    public async Task Execute_Calculates_Page_Correctly(int tvMazeId, int page) {
        context.GetHighestTvMazeId().Returns(tvMazeId);

        _ = await handler.Execute();

        await client.Received().GetShows(page);
    }

    [Fact]
    public async Task Execute_Imports_Shows() {
        var showDto = new ShowDto() {
            Id = 5,
            Name = "De TV Show",
            Language = "Dutch",
            Premiered = new DateTime(2035, 4, 15),
            Summary = "Dit is een TV show, en ook best een leuke. We moeten er nog wel even op wachten!",
            Genres = new() { "Action", "Comedy" }
        };

        client.GetShows(default).ReturnsForAnyArgs(new List<ShowDto>() { showDto });

        var result = await handler.Execute();

        result.Value.Should().Be(1);

        context.Shows.Received().Add(Arg.Is<Show>(show => show.Name == showDto.Name
                                                       && show.Language == showDto.Language
                                                       && show.Premiered == showDto.Premiered
                                                       && show.Summary == showDto.Summary
                                                       && showDto.Genres.All(name => show.Genres.Any(genre => genre.Name == name))
                                                       && show.TvMazeId == showDto.Id));
        await unitOfWork.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
