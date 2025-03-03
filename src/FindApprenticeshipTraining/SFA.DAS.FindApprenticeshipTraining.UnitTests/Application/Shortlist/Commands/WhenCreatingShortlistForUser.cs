using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands;

public class WhenCreatingShortlistForUser
{
    [Test, MoqAutoData]
    public async Task Then_Creates_The_Shortlist_From_The_Request_Calling_Shortlist_Api(
        PostShortListResponse expectedResult,
        CreateShortlistForUserCommand command,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        CreateShortlistForUserCommandHandler sut)
    {
        //Arrange
        roatpApiClient
            .Setup(x => x.PostWithResponseCode<PostShortListResponse>(It.IsAny<PostShortlistForUserRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<PostShortListResponse>(expectedResult, HttpStatusCode.Created, ""));

        //Act
        PostShortListResponse actualResult = await sut.Handle(command, CancellationToken.None);

        //Assert
        roatpApiClient.Verify(x => x
            .PostWithResponseCode<PostShortListResponse>(It.Is<PostShortlistForUserRequest>(c =>
                ((PostShortlistData)c.Data).Latitude.Equals(command.Lat)
                && ((PostShortlistData)c.Data).Longitude.Equals(command.Lon)
                && ((PostShortlistData)c.Data).Ukprn.Equals(command.Ukprn)
                && ((PostShortlistData)c.Data).LocationDescription.Equals(command.LocationDescription)
                && ((PostShortlistData)c.Data).LarsCode.Equals(command.LarsCode)
                && ((PostShortlistData)c.Data).UserId.Equals(command.ShortlistUserId)), false));
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
