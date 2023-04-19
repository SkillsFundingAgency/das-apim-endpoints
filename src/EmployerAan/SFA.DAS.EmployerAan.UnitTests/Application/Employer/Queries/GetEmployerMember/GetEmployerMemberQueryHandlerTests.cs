using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.EmployerAan.InnerApi.Requests;
using SFA.DAS.EmployerAan.Services;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Employer.Queries.GetEmployerMember;
public class GetEmployerMemberQueryHandlerTests
{
    [Test]
    [AutoData]
    public async Task Handle_OkApiResponse_ReturnsResult(
        GetEmployerMemberQuery query,
        GetEmployerMemberQueryResult expectedResponse)
    {
        Mock<IAanHubApiClient<AanHubApiConfiguration>> apiclientMock = new();
        apiclientMock.Setup(c => c.GetWithResponseCode<GetEmployerMemberQueryResult>(It.Is<GetEmployerMemberRequest>(r => r.UserRef == query.UserRef))).ReturnsAsync(new ApiResponse<GetEmployerMemberQueryResult>(expectedResponse, HttpStatusCode.OK, null));
        GetEmployerMemberQueryHandler sut = new(apiclientMock.Object);
        var result = await sut.Handle(query, It.IsAny<CancellationToken>());
        result.Should().Be(expectedResponse);
    }

    [Test]
    [AutoData]
    public async Task Handle_NotFoundApiResponse_ReturnsNull(
        GetEmployerMemberQuery query)
    {
        Mock<IAanHubApiClient<AanHubApiConfiguration>> apiclientMock = new();
        apiclientMock.Setup(c => c.GetWithResponseCode<GetEmployerMemberQueryResult>(It.Is<GetEmployerMemberRequest>(r => r.UserRef == query.UserRef))).ReturnsAsync(new ApiResponse<GetEmployerMemberQueryResult>(null!, HttpStatusCode.NotFound, null));
        GetEmployerMemberQueryHandler sut = new(apiclientMock.Object);
        var result = await sut.Handle(query, It.IsAny<CancellationToken>());
        result.Should().BeNull();
    }


    [Test]
    [AutoData]
    public async Task Handle_UnexpectedApiResponse_ThrowsException(
        GetEmployerMemberQuery query)
    {
        Mock<IAanHubApiClient<AanHubApiConfiguration>> apiclientMock = new();
        apiclientMock.Setup(c => c.GetWithResponseCode<GetEmployerMemberQueryResult>(It.Is<GetEmployerMemberRequest>(r => r.UserRef == query.UserRef))).ReturnsAsync(new ApiResponse<GetEmployerMemberQueryResult>(null!, HttpStatusCode.InternalServerError, null));
        GetEmployerMemberQueryHandler sut = new(apiclientMock.Object);
        Func<Task> action = () => sut.Handle(query, It.IsAny<CancellationToken>());
        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
