using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Admins.Commands.Create;
using SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class AdminsControllerTests
{
    [Test, MoqAutoData]
    public async Task LookupMemberWithDetailPresent_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        LookupAdminMemberRequest request,
        LookupAdminMemberResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(request, cancellationToken)).ReturnsAsync(expected);

        var response = await sut.Lookup(request, cancellationToken);

        mediatorMock.Verify(m => m.Send(request, cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }


    [Test, MoqAutoData]
    public async Task LookupMemberWithDetailAbsent_InvokesCreateAdminMember(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        LookupAdminMemberRequest lookupRequest,
        CreateAdminMemberCommandResult createAdminMemberCommandResult,
        CancellationToken cancellationToken)
    {
        var createAdminMemberCommand = new CreateAdminMemberCommand
        {
            Email = lookupRequest.Email,
            FirstName = lookupRequest.FirstName,
            LastName = lookupRequest.LastName
        };

        mediatorMock.Setup(m => m.Send(lookupRequest, cancellationToken)).ReturnsAsync((LookupAdminMemberResult?)null);
        mediatorMock.Setup(m => m.Send(It.Is<CreateAdminMemberCommand>(
            r => r.Email == lookupRequest.Email
                                        && r.FirstName == lookupRequest.FirstName
                                        && r.LastName == lookupRequest.LastName),
            cancellationToken)).ReturnsAsync(createAdminMemberCommandResult);

        var response = await sut.Lookup(lookupRequest, cancellationToken);

        mediatorMock.Verify(m => m.Send(lookupRequest, cancellationToken));
        var expected = new LookupAdminMemberResult
        {
            MemberId = createAdminMemberCommandResult.MemberId,
            Status = Constants.Status.Live
        };

        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}