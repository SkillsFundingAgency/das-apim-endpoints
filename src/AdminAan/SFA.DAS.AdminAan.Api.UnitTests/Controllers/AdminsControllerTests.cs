using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
using SFA.DAS.AdminAan.Infrastructure.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;
public class AdminsControllerTests
{
    [Test, MoqAutoData]
    public async Task LookupMember_MemberExists_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        LookupAdminMemberRequest request,
        LookupAdminMemberResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<LookupAdminMemberRequestModel>(x => x.Email == request.Email), cancellationToken)).ReturnsAsync(expected);

        var response = await sut.Lookup(request, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<LookupAdminMemberRequestModel>(x => x.Email == request.Email), cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task LookupMember_MemberDoesNotExist_InvokesCreateAdminMember(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        LookupAdminMemberRequest lookupRequest,
        CreateAdminMemberCommandResult createAdminMemberCommandResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<LookupAdminMemberRequestModel>(), cancellationToken)).ReturnsAsync((LookupAdminMemberResult?)null);
        mediatorMock.Setup(m => m.Send(It.Is<CreateAdminMemberCommand>(
                r => r.Email == lookupRequest.Email
                     && r.FirstName == lookupRequest.FirstName
                     && r.LastName == lookupRequest.LastName),
            cancellationToken)).ReturnsAsync(createAdminMemberCommandResult);

        var response = await sut.Lookup(lookupRequest, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<LookupAdminMemberRequestModel>(x => x.Email == lookupRequest.Email), cancellationToken));

        var expected = new LookupAdminMemberResult
        {
            MemberId = createAdminMemberCommandResult.MemberId,
            Status = Constants.Status.Live.GetDescription()
        };

        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
