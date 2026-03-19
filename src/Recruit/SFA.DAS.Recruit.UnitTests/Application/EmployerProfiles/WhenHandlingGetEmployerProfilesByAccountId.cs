using SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfilesByAccountId;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Application.EmployerProfiles;

[TestFixture]
internal class WhenHandlingGetEmployerProfilesByAccountId
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        GetEmployerProfilesByAccountIdQuery query,
        List<EmployerProfile> response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetEmployerProfilesByAccountIdQueryHandler sut)
    {
        // arrange
        GetEmployerProfilesByAccountIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<List<EmployerProfile>> (It.IsAny<GetEmployerProfilesByAccountIdApiRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetEmployerProfilesByAccountIdApiRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.EmployerProfiles.Should().BeEquivalentTo(response);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.AccountId.Should().Be(query.AccountId);
    }
}
