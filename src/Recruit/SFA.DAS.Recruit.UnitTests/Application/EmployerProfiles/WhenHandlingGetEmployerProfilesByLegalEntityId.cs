using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfileByLegalEntityId;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.UnitTests.Application.EmployerProfiles;

[TestFixture]
internal class WhenHandlingGetEmployerProfilesByLegalEntityId
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        GetEmployerProfileByLegalEntityIdQuery query,
        EmployerProfile response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetEmployerProfileByLegalEntityIdQueryHandler sut)
    {
        // arrange
        GetEmployerProfileByLegalEntityIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<EmployerProfile> (It.IsAny<GetEmployerProfileByLegalEntityIdApiRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetEmployerProfileByLegalEntityIdApiRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.EmployerProfile.Should().BeEquivalentTo(response);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.AccountLegalEntityId.Should().Be(query.AccountLegalEntityId);
        capturedRequest.GetUrl.Should().NotBeNull();
    }
}
