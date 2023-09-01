using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MembersControllerTests;

public class GetMembersTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        CancellationToken cancellationToken)
    {
        var model = new GetMembersRequestModel
        {
            Keyword = null,
            UserType = new List<MemberUserType>(),
            IsRegionalChair = null,
            RegionId = new List<int>()
        };

        await sut.GetMembers(model, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetMembersQuery>(q => q.Keyword == null),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        List<MemberUserType> userType,
        bool? isRegionalChair,
        List<int> regionIds,
        string keyword,
        int? page,
        int? pageSize,
        GetMembersQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMembersQuery>(q => q.Keyword == keyword), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var model = new GetMembersRequestModel
        {
            Keyword = keyword,
            UserType = userType,
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