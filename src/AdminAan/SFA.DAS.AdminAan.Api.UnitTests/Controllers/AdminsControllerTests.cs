using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Api.Models.Admins;
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
        LookupAdminMemberRequestModel requestModel,
        LookupAdminMemberResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<LookupAdminMemberRequest>(x => x.Email == requestModel.Email), cancellationToken)).ReturnsAsync(expected);

        var response = await sut.Lookup(requestModel, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<LookupAdminMemberRequest>(x => x.Email == requestModel.Email), cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task LookupMember_MemberDoesNotExist_InvokesCreateAdminMember(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        LookupAdminMemberRequestModel lookupRequestModel,
        CreateAdminMemberCommandResult createAdminMemberCommandResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<LookupAdminMemberRequest>(), cancellationToken)).ReturnsAsync((LookupAdminMemberResult?)null);
        mediatorMock.Setup(m => m.Send(It.Is<CreateAdminMemberCommand>(
                r => r.Email == lookupRequestModel.Email
                     && r.FirstName == lookupRequestModel.FirstName
                     && r.LastName == lookupRequestModel.LastName),
            cancellationToken)).ReturnsAsync(createAdminMemberCommandResult);

        var response = await sut.Lookup(lookupRequestModel, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<LookupAdminMemberRequest>(x => x.Email == lookupRequestModel.Email), cancellationToken));

        var expected = new LookupAdminMemberResult
        {
            MemberId = createAdminMemberCommandResult.MemberId,
            Status = Constants.Status.Live.GetDescription()
        };

        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
