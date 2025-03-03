using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteSavedSearch;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;

public class WhenHandlingDeleteSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Deleted(
        DeleteSavedSearchCommand command,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        DeleteSavedSearchCommandHandler sut)
    {
        // act
        await sut.Handle(command, default);
        
        // assert
        apiClient.Verify(x => 
            x.Delete(It.Is<IDeleteApiRequest>(r => r.DeleteUrl.Equals($"api/Users/{command.CandidateId}/SavedSearches/{command.Id}"))), Times.Once);
    }
}