using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Commands;

public class WhenDeletingShortlistItem
{
    [Test, MoqAutoData]
    public async Task Then_Deletes_The_Shortlist_Item_From_The_Request_Calling_Shortlist_Api(
        DeleteShortlistItemCommand command,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApi,
        DeleteShortlistItemCommandHandler sut)
    {
        roatpApi.Setup(x => x.DeleteWithResponseCode<DeleteShortlistItemCommandResult>(It.IsAny<DeleteShortlistItemRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortlistItemCommandResult>(new DeleteShortlistItemCommandResult(true), HttpStatusCode.Accepted, null));

        //Act
        await sut.Handle(command, CancellationToken.None);

        //Assert
        roatpApi.Verify(x =>
            x.DeleteWithResponseCode<DeleteShortlistItemCommandResult>(It.Is<DeleteShortlistItemRequest>(c =>
                c.DeleteUrl.Equals($"api/shortlists/{command.ShortlistId}")), true), Times.Once);
    }
}
