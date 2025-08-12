using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Commands;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderContactControllerTests;
public class WhenPostingProviderContact
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Command_Is_Handled_And_Created_Status_Returned(
        int ukprn,
        CreateProviderContactCommand command,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderContactCreateController sut)
    {
        var statusCode = (int)HttpStatusCode.Created;

        mediator.Setup(x => x.Send(It.Is<CreateProviderContactCommand>(c => c.Ukprn == ukprn && c.EmailAddress == command.EmailAddress && c.PhoneNumber == command.PhoneNumber && c.UserDisplayName == command.UserDisplayName && c.UserId == command.UserId), new CancellationToken())).ReturnsAsync(statusCode);

        var actual = await sut.CreateProviderContact(ukprn, command) as IStatusCodeActionResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be(statusCode);
    }

    [Test, MoqAutoData]
    public async Task Then_Mediator_Command_Is_Handled_And_BadRequest_Status_Returned(
        int ukprn,
        CreateProviderContactCommand command,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderContactCreateController sut)
    {
        var statusCode = (int)HttpStatusCode.BadRequest;

        mediator.Setup(x => x.Send(It.Is<CreateProviderContactCommand>(c => c.Ukprn == ukprn && c.EmailAddress == command.EmailAddress && c.PhoneNumber == command.PhoneNumber && c.UserDisplayName == command.UserDisplayName && c.UserId == command.UserId), new CancellationToken())).ReturnsAsync(statusCode);

        var actual = await sut.CreateProviderContact(ukprn, command) as IStatusCodeActionResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be(statusCode);
    }
}
