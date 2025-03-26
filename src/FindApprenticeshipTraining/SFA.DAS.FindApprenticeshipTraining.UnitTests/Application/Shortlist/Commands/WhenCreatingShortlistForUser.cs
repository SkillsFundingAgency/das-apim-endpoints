using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;
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
        LocationItem locationItem,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
        CreateShortlistForUserCommandHandler sut
    )
    {
        //Arrange
        roatpApiClient
            .Setup(x => x.PostWithResponseCode<PostShortListResponse>(It.IsAny<PostShortlistForUserRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<PostShortListResponse>(expectedResult, HttpStatusCode.Created, ""));

        cachedLocationLookupService
            .Setup(a => a.GetCachedLocationInformation(command.LocationName, false))
            .ReturnsAsync(locationItem);

        //Act
        PostShortListResponse actualResult = await sut.Handle(command, CancellationToken.None);

        //Assert
        roatpApiClient.Verify(x => x
            .PostWithResponseCode<PostShortListResponse>(It.Is<PostShortlistForUserRequest>(c =>
                   ((PostShortlistData)c.Data).Ukprn.Equals(command.Ukprn)
                && ((PostShortlistData)c.Data).Latitude.Equals(locationItem.Latitude)
                && ((PostShortlistData)c.Data).Longitude.Equals(locationItem.Longitude)
                && ((PostShortlistData)c.Data).LocationDescription.Equals(command.LocationName)
                && ((PostShortlistData)c.Data).LarsCode.Equals(command.LarsCode)
                && ((PostShortlistData)c.Data).UserId.Equals(command.ShortlistUserId)), true));
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test, MoqAutoData]
    public async Task When_Location_Is_Null_Then_Shortlist_Is_Created_With_No_Longitude_And_Latitude(
        PostShortListResponse expectedResult,
        CreateShortlistForUserCommand command,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
        CreateShortlistForUserCommandHandler sut
    )
    {
        //Arrange
        roatpApiClient
            .Setup(x => x.PostWithResponseCode<PostShortListResponse>(It.IsAny<PostShortlistForUserRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<PostShortListResponse>(expectedResult, HttpStatusCode.Created, ""));

        cachedLocationLookupService
            .Setup(a => a.GetCachedLocationInformation(command.LocationName, false))
            .ReturnsAsync((LocationItem)null);

        //Act
        PostShortListResponse actualResult = await sut.Handle(command, CancellationToken.None);

        //Assert
        roatpApiClient.Verify(x => x
            .PostWithResponseCode<PostShortListResponse>(It.Is<PostShortlistForUserRequest>(c =>
                   ((PostShortlistData)c.Data).Ukprn.Equals(command.Ukprn)
                && ((PostShortlistData)c.Data).Latitude.Equals(null)
                && ((PostShortlistData)c.Data).Longitude.Equals(null)
                && ((PostShortlistData)c.Data).LocationDescription.Equals(command.LocationName)
                && ((PostShortlistData)c.Data).LarsCode.Equals(command.LarsCode)
                && ((PostShortlistData)c.Data).UserId.Equals(command.ShortlistUserId)), true));
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
