using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands
{
    public class WhenDeletingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Deletes_The_Shortlist_Item_From_The_Request_Calling_CourseDelivery_Api(
            DeleteShortlistForUserCommand command,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> _apiClient,
            DeleteShortlistForUserCommandHandler handler)
        {
            //Act
            await handler.Handle(command, CancellationToken.None);
        
            //Assert
            _apiClient.Verify(x=>
                x.Delete(It.Is<DeleteShortlistForUserRequest>(c=>
                    c.DeleteUrl.Equals($"api/shortlist/users/{command.UserId}"))), Times.Once);
        }
    }
}