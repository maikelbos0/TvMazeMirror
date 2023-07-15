using FluentAssertions;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Database;
using TvMazeMirror.Entities;
using TvMazeMirror.Models;
using Xunit;

namespace TvMazeMirror.Tests.CommandHandlers;

public class AddShowCommandHandlerTests {
    private readonly ITvMazeContext context = Substitute.For<ITvMazeContext>();
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly AddShowCommandHandler handler;

    public AddShowCommandHandlerTests() {
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
    public async Task Execute_Adds_Show_Correctly() {
        var model = new ShowModel() {
            Name = "De TV Show",
            Language = "Dutch",
            Premiered = new DateTime(2035, 4, 15),
            Summary = "Dit is een TV show, en ook best een leuke. We moeten er nog wel even op wachten!",
            Genres = new() { "Action", "Comedy" },
            TvMazeId = 5
        };

        var result = await handler.Execute(model);

        result.IsValid.Should().BeTrue();

        context.Shows.Received().Add(Arg.Is<Show>(show => show.Name == model.Name
                                                       && show.Language == model.Language
                                                       && show.Premiered == model.Premiered
                                                       && show.Summary == model.Summary
                                                       && model.Genres.All(name => show.Genres.Any(genre => genre.Name == name))
                                                       && show.TvMazeId == null));
        await unitOfWork.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
