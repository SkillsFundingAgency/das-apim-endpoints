using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using System.Net;
using FluentAssertions;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;
public class WhenGettingUsersByEmail
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Users_With_Matching_Emails(
        GetUsersByEmailQuery mockQuery,
        GetUsersByEmailResponse mockApiResponse,
        [Frozen] Mock<IInternalApiClient<EmployerProfilesApiConfiguration>> mockApiClient,
        GetUsersByEmailQueryHandler sut)
    {
        var expectedUrl = $"api/users/query?email={WebUtility.UrlEncode(mockQuery.Email)}";
        mockApiClient.Setup(client => client.Get<GetUsersByEmailResponse>(It.Is<GetUsersByEmailRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Users.Should().BeEquivalentTo(mockApiResponse.Users);
    }
}

