using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistCountForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Queries;

public class WhenGettingShortlistCountForUser
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_The_Request(
        GetShortlistCountForUserQueryResult result,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClientMock,
        GetShortlistCountForUserQueryHandler sut)
    {
        //Arrange
        var userId = Guid.NewGuid();
        var request = new GetShortlistCountForUserQuery(userId);
        var apiResponse = new ApiResponse<GetShortlistCountForUserQueryResult>(result, HttpStatusCode.OK, null);
        roatpCourseManagementApiClientMock.Setup(x => x.GetWithResponseCode<GetShortlistCountForUserQueryResult>(It.Is<GetShortlistCountForUserRequest>(c => c.UserId.Equals(userId))))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await sut.Handle(request, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(result);
    }
}
