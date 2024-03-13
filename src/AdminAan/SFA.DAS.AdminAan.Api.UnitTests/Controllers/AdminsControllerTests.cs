using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Api.Models.Admins;
using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.GetAdminMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;
public class AdminsControllerTests
{
    [Test, MoqAutoData]
    public async Task LookupMember_MemberExists_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        GetAdminMemberRequestModel requestModel,
        GetAdminMemberResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberRequest>(x => x.Email == requestModel.Email), cancellationToken)).ReturnsAsync(expected);

        var response = await sut.Lookup(requestModel, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetAdminMemberRequest>(x => x.Email == requestModel.Email), cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task LookupMember_MemberDoesNotExist_InvokesCreateAdminMember(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        GetAdminMemberRequestModel requestModel,
        CreateAdminMemberCommandResult createAdminMemberCommandResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAdminMemberRequest>(), cancellationToken)).ReturnsAsync((GetAdminMemberResult?)null);
        mediatorMock.Setup(m => m.Send(It.Is<CreateAdminMemberCommand>(
                r => r.Email == requestModel.Email
                     && r.FirstName == requestModel.FirstName
                     && r.LastName == requestModel.LastName),
            cancellationToken)).ReturnsAsync(createAdminMemberCommandResult);

        var response = await sut.Lookup(requestModel, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetAdminMemberRequest>(x => x.Email == requestModel.Email), cancellationToken));

        var expected = new GetAdminMemberResult
        {
            MemberId = createAdminMemberCommandResult.MemberId,
            Status = AdminsController.LiveStatus
        };

        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
