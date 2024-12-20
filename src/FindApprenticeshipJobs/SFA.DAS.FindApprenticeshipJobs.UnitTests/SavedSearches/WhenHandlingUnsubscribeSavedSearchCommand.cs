using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.UnsubscribeSavedSearch;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.SavedSearches;

public class WhenHandlingUnsubscribeSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Delete_Called_With_Id(
        UnsubscribeSavedSearchCommand command,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> client,
        UnsubscribeSavedSearchCommandHandler handler)
    {
        await handler.Handle(command, CancellationToken.None);

        client.Verify(x => x.Delete(It.Is<DeleteSavedSearchRequest>(c => c.Id.Equals(command.SavedSearchId))), Times.Once);
    }
}