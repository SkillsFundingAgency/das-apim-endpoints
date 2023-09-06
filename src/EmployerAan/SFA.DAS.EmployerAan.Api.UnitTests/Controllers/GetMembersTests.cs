using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Application.Models;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MembersControllerTests;
public class GetMembersTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var model = new GetMembersRequestModel
        {
            RequestedByMemberId = requestedByMemberId,
            Keyword = null,
            UserType = new List<MemberUserType>(),
            Status = new List<MembershipStatusType>(),
            IsRegionalChair = null,
            RegionId = new List<int>()
        };

        await sut.GetMembers(model, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetMembersQuery>(q => q.RequestedByMemberId == requestedByMemberId),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid requestedByMemberId,
        List<MemberUserType> userType,
        List<MembershipStatusType> status,
        bool? isRegionalChair,
        List<int> regionIds,
        string keyword,
        int? page,
        int? pageSize,
        GetMembersQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMembersQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var model = new GetMembersRequestModel
        {
            RequestedByMemberId = requestedByMemberId,
            Keyword = keyword,
            UserType = userType,
            Status = status,
            IsRegionalChair = isRegionalChair,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };
        var result = await sut.GetMembers(model, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
