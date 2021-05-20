using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands
{
    public class WhenCreatingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Creates_The_Shortlist_From_The_Request_Calling_CourseDelivery_Api(
            PostShortListResponse apiResponse,
            CreateShortlistForUserCommand command,
            GetStandardsListItem standardApiResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> courseDeliveryApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            CreateShortlistForUserCommandHandler handler)
        {
            //Arrange
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c =>
                    c.StandardId.Equals(command.StandardId)))).ReturnsAsync(standardApiResponse);
            courseDeliveryApiClient.Setup(x=>
                x.PostWithResponseCode<PostShortListResponse>(It.Is<PostShortlistForUserRequest>(c=>
                    ((PostShortlistData)c.Data).Lat.Equals(command.Lat)
                    && ((PostShortlistData)c.Data).Lon.Equals(command.Lon)
                    && ((PostShortlistData)c.Data).Ukprn.Equals(command.Ukprn)
                    && ((PostShortlistData)c.Data).LocationDescription.Equals(command.LocationDescription)
                    && ((PostShortlistData)c.Data).StandardId.Equals(command.StandardId)
                    && ((PostShortlistData)c.Data).SectorSubjectArea.Equals(standardApiResponse.SectorSubjectAreaTier2Description)
                    && ((PostShortlistData)c.Data).ShortlistUserId.Equals(command.ShortlistUserId)))).ReturnsAsync(new ApiResponse<PostShortListResponse>(apiResponse, HttpStatusCode.Accepted, ""));
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);

            //Assert
            actual.Should().Be(apiResponse.Id);
        }
    }
}