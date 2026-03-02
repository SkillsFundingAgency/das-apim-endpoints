using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ReferenceData;

public class WhenGettingCandidateQualifications
{
    [Test, MoqAutoData]
    public async Task Then_The_Skills_Are_Returned(
        List<string> expectedQualifications,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] ReferenceDataController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<List<string>>(It.IsAny<GetCandidateQualificationsRequest>()))
            .ReturnsAsync(expectedQualifications);

        // act
        var response = await sut.GetCandidateQualifications(recruitApiClient.Object) as Ok<List<string>>;
        var payload = response?.Value;

        // assert
        payload.Should().BeEquivalentTo(expectedQualifications);
    }
}