using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using Xunit;

namespace TvMazeMirror.Tests.CommandHandlers;

public class DeleteShowCommandHandlerTests {
    private readonly ITvMazeContext context = Substitute.For<ITvMazeContext>();
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteShowCommandHandler handler;

    public DeleteShowCommandHandlerTests() {
        handler = new(context, unitOfWork);
    }

    [Fact]
    public async Task Execute_Show_Must_Exist() {
        context.FindShow(default).ReturnsForAnyArgs((Show?)null);

        var result = await handler.Execute(1);

        result.IsFound.Should().BeFalse();

        context.Shows.DidNotReceiveWithAnyArgs().Remove(default!);
        await unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }

    [Fact]
    public async Task Execute_Deletes_Show() {
        var show = new Show("De vorige TV Show") {
            Id = 1,
            Language = "Danish",
            Premiered = new DateTime(2135, 4, 15),
            Summary = "Dit klinkt eigenlijk niet echt als Deens?",
            Genres = new List<ShowGenre>() {
                new ShowGenre("Comedy"),
                new ShowGenre("Drama")
            },
            TvMazeId = 15
        };

        context.FindShow(default).ReturnsForAnyArgs(show);

        var result = await handler.Execute(1);

        result.IsValid.Should().BeTrue();
        result.IsFound.Should().BeTrue();

        context.Shows.Received().Remove(show);        
        await unitOfWork.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
