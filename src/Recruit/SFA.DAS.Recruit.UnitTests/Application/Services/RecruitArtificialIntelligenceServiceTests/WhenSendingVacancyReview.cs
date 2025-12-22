using System.Linq;
using System.Text.Json;
using SFA.DAS.Recruit.Application.Services;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Recruit.UnitTests.Application.Services.RecruitArtificialIntelligenceServiceTests;

public class WhenSendingVacancyReview
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Review_Data_Is_Sent_In_The_Correct_Format(
        Vacancy vacancy,
        GetStandardsListResponse standardsResponse,
        [Frozen] Mock<ICourseService> coursesClient,
        [Frozen] Mock<IRecruitArtificialIntelligenceClient> aiClient,
        [Greedy] RecruitArtificialIntelligenceService sut)
    {
        // arrange
        var standard = standardsResponse.Standards.First();
        vacancy.ProgrammeId = standard.LarsCode.ToString();
        foreach (var getStandardsListItem in standardsResponse.Standards)
        {
            getStandardsListItem.Level = 3;
        }
        coursesClient
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(standardsResponse);

        AiVacancyReviewData? capturedRequest = null;
        aiClient
            .Setup(x => x.SendPayloadAsync(It.IsAny<It.IsAnyType>(), CancellationToken.None))
            .Callback<object, CancellationToken>((x, _) => capturedRequest = x as AiVacancyReviewData);

        // act
        await sut.SendVacancyReviewAsync(vacancy, CancellationToken.None);

        // assert
        aiClient.Verify(x => x.SendPayloadAsync(It.IsAny<It.IsAnyType>(), CancellationToken.None), Times.Once);
        capturedRequest!.Should().NotBeNull();
        capturedRequest!.VacancyId.Should().Be(vacancy.Id);
        capturedRequest.Title.Should().Be(vacancy.Title);
        capturedRequest.ShortDescription.Should().Be(vacancy.ShortDescription);
        capturedRequest.Description.Should().Be(vacancy.Description);
        capturedRequest.EmployerDescription.Should().Be(vacancy.EmployerDescription);
        capturedRequest.Skills.Should().Be(JsonSerializer.Serialize(vacancy.Skills, Global.JsonSerializerOptionsCaseInsensitive));
        capturedRequest.Qualifications.Should().Be(JsonSerializer.Serialize(vacancy.Qualifications, Global.JsonSerializerOptionsCaseInsensitive));
        capturedRequest.ThingsToConsider.Should().Be(vacancy.ThingsToConsider);
        capturedRequest.TrainingDescription.Should().Be(vacancy.TrainingDescription);
        capturedRequest.AdditionalTrainingDescription.Should().Be(vacancy.AdditionalTrainingDescription);
        capturedRequest.TrainingProgrammeTitle.Should().Be(standard.Title);
        capturedRequest.TrainingProgrammeLevel.Should().Be("Level 3");
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Data_Is_Not_Sent_If_The_Standard_Cannot_Be_Found(
        Vacancy vacancy,
        GetStandardsListResponse standardsResponse,
        [Frozen] Mock<ICourseService> coursesClient,
        [Frozen] Mock<IRecruitArtificialIntelligenceClient> aiClient,
        [Greedy] RecruitArtificialIntelligenceService sut)
    {
        // arrange
        foreach (var getStandardsListItem in standardsResponse.Standards)
        {
            getStandardsListItem.Level = 3;
        }
        coursesClient
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(standardsResponse);

        // act
        await sut.SendVacancyReviewAsync(vacancy, CancellationToken.None);

        // assert
        aiClient.Verify(x => x.SendPayloadAsync(It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}