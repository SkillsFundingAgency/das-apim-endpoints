using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.SaveSearch;

public class WhenHandlingUnsubscribeSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Delete_Called_With_Id(
        UnsubscribeSavedSearchCommand command,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> client,
        UnsubscribeSavedSearchCommandHandler handler)
    {
        await handler.Handle(command, CancellationToken.None);

        client.Verify(x => x.Delete(It.Is<DeleteSavedSearchRequest>(c => c.Id.Equals(command.Id))), Times.Once);
    }
}