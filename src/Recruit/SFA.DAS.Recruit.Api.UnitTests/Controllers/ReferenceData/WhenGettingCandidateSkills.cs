using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ReferenceData;

public class WhenGettingCandidateSkills
{
    [Test, MoqAutoData]
    public async Task Then_The_Skills_Are_Returned(
        List<string> expectedSkills,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] ReferenceDataController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<List<string>>(It.IsAny<GetCandidateSkillsRequest>()))
            .ReturnsAsync(expectedSkills);

        // act
        var response = await sut.GetCandidateSkills(recruitApiClient.Object) as Ok<List<string>>;
        var payload = response?.Value;

        // assert
        payload.Should().BeEquivalentTo(expectedSkills);
    }
}