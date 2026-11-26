using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands;

public class CreateShortlistForUserCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_WhenCalled_CreatesShortlistAndCallsRoatpApi(
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
                && ((PostShortlistData)c.Data).LocationName.Equals(command.LocationName)
                && ((PostShortlistData)c.Data).LarsCode.Equals(command.LarsCode)
                && ((PostShortlistData)c.Data).UserId.Equals(command.ShortlistUserId)), true));
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenLocationNotFound_SetsLatitudeAndLongitudeToNull(
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
                && ((PostShortlistData)c.Data).LocationName.Equals(command.LocationName)
                && ((PostShortlistData)c.Data).LarsCode.Equals(command.LarsCode)
                && ((PostShortlistData)c.Data).UserId.Equals(command.ShortlistUserId)), true));
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}