using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands;

public class WhenDeletingShortlistItemForUser
{
    [Test, MoqAutoData]
    public async Task Then_Deletes_The_Shortlist_Item_From_The_Request_Calling_Shortlist_Api(
        DeleteShortlistItemCommand command,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApi,
        DeleteShortlistItemCommandHandler sut)
    {
        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        roatpApi.Verify(x =>
            x.Delete(It.Is<DeleteShortlistItemRequest>(c =>
                c.DeleteUrl.Equals($"api/shortlists/{command.ShortlistId}"))), Times.Once);
    }
}
