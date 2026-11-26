using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PostEmployerProfile;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.EmployerProfiles;

[TestFixture]
internal class WhenHandlingPostEmployerProfilesCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        PostEmployerProfileCommand command,
        Recruit.InnerApi.Models.EmployerProfile response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] PostEmployerProfileCommandHandler sut)
    {
        // arrange
        var expectedPutUrl = new PostEmployerProfileApiRequest(command.AccountLegalEntityId, new PostEmployerProfileApiRequest.PostEmployerProfileApiRequestData
        {
            AccountId = command.AccountId,
            TradingName = command.TradingName,
            AboutOrganisation = command.AboutOrganisation
        });

        recruitApiClient.Setup(x => x.PutWithResponseCode<Recruit.InnerApi.Models.EmployerProfile>(
                It.Is<PostEmployerProfileApiRequest>(r => r.PutUrl == expectedPutUrl.PutUrl)))
            .ReturnsAsync(new ApiResponse<Recruit.InnerApi.Models.EmployerProfile>(response, HttpStatusCode.OK, string.Empty));

        // act
        await sut.Handle(command, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PutWithResponseCode<Recruit.InnerApi.Models.EmployerProfile>(It.IsAny<PostEmployerProfileApiRequest>()), Times.Once);
    }
}
