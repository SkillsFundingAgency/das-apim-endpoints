using System.Net;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands
{
    public class WhenCreatingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Creates_The_Shortlist_From_The_Request_Calling_Shortlist_Api(
            PostShortListResponse apiResponse,
            CreateShortlistForUserCommand command,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> shortlistApiClient,
            CreateShortlistForUserCommandHandler handler)
        {
            //Arrange
            shortlistApiClient.Setup(x => x
                    .PostWithResponseCode<PostShortListResponse>(It.IsAny<PostShortlistForUserRequest>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new ApiResponse<PostShortListResponse>(null, HttpStatusCode.Accepted, ""));
            
            //Act
            await handler.Handle(command, CancellationToken.None);
        
            //Assert
            shortlistApiClient.Verify(x => x
                .PostWithResponseCode<PostShortListResponse>(It.Is<PostShortlistForUserRequest>(c =>
                    ((PostShortlistData)c.Data).Latitude.Equals(command.Lat)
                    && ((PostShortlistData)c.Data).Longitude.Equals(command.Lon)
                    && ((PostShortlistData)c.Data).Ukprn.Equals(command.Ukprn)
                    && ((PostShortlistData)c.Data).LocationDescription.Equals(command.LocationDescription)
                    && ((PostShortlistData)c.Data).Larscode.Equals(command.StandardId)
                    && ((PostShortlistData)c.Data).ShortlistUserId.Equals(command.ShortlistUserId)), false));
        }
    }
}