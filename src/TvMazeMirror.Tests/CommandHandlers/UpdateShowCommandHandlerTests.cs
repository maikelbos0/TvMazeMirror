using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Models;
using Xunit;

namespace TvMazeMirror.Tests.CommandHandlers;

public class UpdateShowCommandHandlerTests {
    private readonly ITvMazeContext context = Substitute.For<ITvMazeContext>();
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateShowCommandHandler handler;

    public UpdateShowCommandHandlerTests() {
        handler = new(context, unitOfWork);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Execute_Name_Is_Required(string name) {
        var result = await handler.Execute(new ShowModel() {
            Name = name
        });

        var error = result.Errors.Should().ContainSingle().Subject;
        error.Property.Should().Be("Name");
        error.Message.Should().Be("Name is required");

        context.Shows.DidNotReceiveWithAnyArgs().Add(default!);
        await unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }

    [Fact]
    public async Task Execute_Show_Must_Exist() {

        var model = new ShowModel() {
            Name = "De TV Show",
            Language = "Dutch",
            Premiered = new DateTime(2035, 4, 15),
            Summary = "Dit is een TV show, en ook best een leuke. We moeten er nog wel even op wachten!",
            Genres = new() { "Action", "Comedy" },
            TvMazeId = 5
        };

        var result = await handler.Execute(model);

        result.IsFound.Should().BeFalse();

        context.Shows.DidNotReceiveWithAnyArgs().Update(default!);
        await unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }

    [Fact]
    public async Task Execute_Updates_Show_Correctly() {
        context.FindShow(default).ReturnsForAnyArgs(new Show("De vorige TV Show") {
            Id = 1,
            Language = "Danish",
            Premiered = new DateTime(2135, 4, 15),
            Summary = "Dit klinkt eigenlijk niet echt als Deens?",
            Genres = new List<ShowGenre>() {
                new ShowGenre("Comedy"),
                new ShowGenre("Drama")
            },
            TvMazeId = 15
        });

        var model = new ShowModel() {
            Id = 1,
            Name = "De TV Show",
            Language = "Dutch",
            Premiered = new DateTime(2035, 4, 15),
            Summary = "Dit is een TV show, en ook best een leuke. We moeten er nog wel even op wachten!",
            Genres = new() { "Action", "Comedy" },
            TvMazeId = 5
        };

        var result = await handler.Execute(model);

        result.IsValid.Should().BeTrue();
        result.IsFound.Should().BeTrue();

        context.Shows.Received().Update(Arg.Is<Show>(show => show.Name == model.Name
                                                          && show.Language == model.Language
                                                          && show.Premiered == model.Premiered
                                                          && show.Summary == model.Summary
                                                          && model.Genres.All(name => show.Genres.Any(genre => genre.Name == name))
                                                          && show.TvMazeId == 15));
        
        await unitOfWork.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
