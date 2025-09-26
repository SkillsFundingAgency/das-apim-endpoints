using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Standards.Queries.GetStandards;

public class WhenGettingStandards
{
    [Test, AutoData]
    public async Task Then_Gets_Standards_From_Courses_Api(GetStandardsListResponse expectedResponse)
    {
        // Arrange
        var query = new GetStandardsQuery();

        var mockApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        mockApiClient
            .Setup(client => client.GetWithResponseCode<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
            .ReturnsAsync(new ApiResponse<GetStandardsListResponse>(expectedResponse, System.Net.HttpStatusCode.OK, null));
        var handler = new GetStandardsQueryHandler(mockApiClient.Object);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Standards.Count.Should().Be(expectedResponse.Standards.Count());
    }
}
