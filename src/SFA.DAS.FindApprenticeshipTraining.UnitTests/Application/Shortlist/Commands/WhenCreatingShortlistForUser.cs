using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands
{
    public class WhenCreatingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Creates_The_Shortlist_From_The_Request_Calling_CourseDelivery_Api(
            CreateShortlistForUserCommand command,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> courseDeliveryApiClient,
            CreateShortlistForUserCommandHandler handler)
        {
            //Act
            await handler.Handle(command, CancellationToken.None);

            //Assert
            courseDeliveryApiClient.Verify(x=>
                x.Post(It.Is<PostShortlistForUserRequest>(c=>
                    c.Data.Lat.Equals(command.Lat)
                    && c.Data.Lon.Equals(command.Lon)
                    && c.Data.Ukprn.Equals(command.Ukprn)
                    && c.Data.LocationDescription.Equals(command.LocationDescription)
                    && c.Data.StandardId.Equals(command.StandardId)
                    && c.Data.SectorSubjectArea.Equals(command.SectorSubjectArea)
                    && c.Data.ShortlistUserId.Equals(command.ShortlistUserId))));
        }
    }
}