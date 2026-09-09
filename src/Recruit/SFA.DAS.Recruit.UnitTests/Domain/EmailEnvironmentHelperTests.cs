using System;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.UnitTests.Domain;

public class EmailEnvironmentHelperTests
{
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Production()
    {
        var actual = new EmailEnvironmentHelper("PRd");
        var vacancyId = Guid.NewGuid();

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("b648c047-b2e3-4ebe-b9b4-a6cc59c2af94");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("8387c857-b4f9-4cfb-8c44-df4c2560e446");
        actual.AdvertApprovedByDfeTemplateId.Should().Be("c35e76e7-303b-4b18-bb06-ad98cf68158d");
        actual.ApplicationSubmittedTemplateId.Should().Be("e07a6992-4d17-4167-b526-2ead6fe9ad4d");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
        string.Format(actual.LiveVacancyUrl,"VAC1000001213").Should().Be("https://findapprenticeship.service.gov.uk/apprenticeship/VAC1000001213");
        string.Format(actual.NotificationsSettingsEmployerUrl,"ABC123").Should().Be("https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/notifications-manage/");
        string.Format(actual.NotificationsSettingsProviderUrl,"10000001").Should().Be("https://recruit.providers.apprenticeships.education.gov.uk/10000001/notifications-manage/");
        string.Format(actual.ReviewVacancyReviewInRecruitEmployerUrl,"ABC123", vacancyId).Should().Be($"https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/vacancies/{vacancyId}/check-answers/");
        string.Format(actual.ManageAdvertUrl,"ABC123", vacancyId).Should().Be($"https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/vacancies/{vacancyId}/manage/");
        string.Format(actual.ReviewVacancyReviewInRecruitProviderUrl,"10000001", vacancyId).Should().Be($"https://recruit.providers.apprenticeships.education.gov.uk/10000001/vacancies/{vacancyId}/check-your-answers/");
    }
    
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Non_Production()
    {
        var actual = new EmailEnvironmentHelper("siT");
        var vacancyId = Guid.NewGuid();

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("35e8b348-5f26-488a-8165-459522f8189b");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("95d7ff0c-79fc-4585-9fff-5e583b478d23");
        actual.AdvertApprovedByDfeTemplateId.Should().Be("c445095e-e659-499b-b2ab-81e321a9b591");
        actual.ApplicationSubmittedTemplateId.Should().Be("8aedd294-fd12-4b77-b4b8-2066744e1fdc");
        string.Format(actual.LiveVacancyUrl,"VAC1000001213").Should().Be("https://sit-findapprenticeship.apprenticeships.education.gov.uk/apprenticeship/VAC1000001213");
        actual.CandidateApplicationUrl.Should().Be("https://sit-findapprenticeship.apprenticeships.education.gov.uk/applications");
        string.Format(actual.NotificationsSettingsEmployerUrl,"ABC123").Should().Be("https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/notifications-manage/");
        string.Format(actual.NotificationsSettingsProviderUrl,"10000001").Should().Be("https://recruit.sit-pas.apprenticeships.education.gov.uk/10000001/notifications-manage/");
        string.Format(actual.ReviewVacancyReviewInRecruitEmployerUrl,"ABC123", vacancyId).Should().Be($"https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/vacancies/{vacancyId}/check-answers/");
        string.Format(actual.ManageAdvertUrl,"ABC123", vacancyId).Should().Be($"https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/vacancies/{vacancyId}/manage/");
        string.Format(actual.ReviewVacancyReviewInRecruitProviderUrl,"10000001", vacancyId).Should().Be($"https://recruit.sit-pas.apprenticeships.education.gov.uk/10000001/vacancies/{vacancyId}/check-your-answers/");
    }
}