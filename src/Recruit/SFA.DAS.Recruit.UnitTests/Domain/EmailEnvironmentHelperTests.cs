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
        actual.VacancyReviewApprovedEmployerTemplateId.Should().Be("d8855c4f-9ce1-4870-93ff-53e609f59a51");
        actual.VacancyReviewApprovedProviderTemplateId.Should().Be("ee2d7ab3-7ac1-47f8-bc32-86290bda55c9");
        actual.VacancyReviewEmployerRejectedByDfeTemplateId.Should().Be("27acd0e9-96fe-47ec-ae33-785e00a453f8");
        actual.VacancyReviewProviderRejectedByDfeTemplateId.Should().Be("872e847b-77f5-44a7-b12e-4a19df969ec1");
        actual.ApplicationSubmittedTemplateId.Should().Be("e07a6992-4d17-4167-b526-2ead6fe9ad4d");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
        string.Format(actual.LiveVacancyUrl,"VAC1000001213").Should().Be("https://findapprenticeship.service.gov.uk/apprenticeship/VAC1000001213");
        string.Format(actual.NotificationsSettingsEmployerUrl,"ABC123").Should().Be("https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/notifications-manage/");
        string.Format(actual.NotificationsSettingsProviderUrl,"10000001").Should().Be("https://recruit.providers.apprenticeships.education.gov.uk/10000001/notifications-manage/");
        string.Format(actual.ReviewVacancyReviewInRecruitEmployerUrl,"ABC123", vacancyId).Should().Be($"https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/vacancies/{vacancyId}/check-answers/");
        string.Format(actual.ReviewVacancyReviewInRecruitProviderUrl,"10000001", vacancyId).Should().Be($"https://recruit.providers.apprenticeships.education.gov.uk/10000001/vacancies/{vacancyId}/check-answers/");
        string.Format(actual.ManageAdvertUrl,"ABC123", vacancyId).Should().Be($"https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/vacancies/{vacancyId}/manage/");
    }
    
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Non_Production()
    {
        var actual = new EmailEnvironmentHelper("siT");
        var vacancyId = Guid.NewGuid();

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("35e8b348-5f26-488a-8165-459522f8189b");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("95d7ff0c-79fc-4585-9fff-5e583b478d23");
        actual.VacancyReviewApprovedEmployerTemplateId.Should().Be("9a45ff1d-769d-4be2-96fb-dcf605e0108f");
        actual.VacancyReviewApprovedProviderTemplateId.Should().Be("48c9ab9e-5b13-4843-b4d5-ee1caa46cc64");
        actual.VacancyReviewEmployerRejectedByDfeTemplateId.Should().Be("5869140a-2a76-4a7c-b4b9-083d2afc5aa5");
        actual.VacancyReviewProviderRejectedByDfeTemplateId.Should().Be("048d93c9-4371-45a3-96c4-3f93241a5908");
        actual.AdvertApprovedByDfeTemplateId.Should().Be("c445095e-e659-499b-b2ab-81e321a9b591");
        actual.ApplicationSubmittedTemplateId.Should().Be("8aedd294-fd12-4b77-b4b8-2066744e1fdc");
        string.Format(actual.LiveVacancyUrl,"VAC1000001213").Should().Be("https://sit-findapprenticeship.apprenticeships.education.gov.uk/apprenticeship/VAC1000001213");
        actual.CandidateApplicationUrl.Should().Be("https://sit-findapprenticeship.apprenticeships.education.gov.uk/applications");
        string.Format(actual.NotificationsSettingsEmployerUrl,"ABC123").Should().Be("https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/notifications-manage/");
        string.Format(actual.NotificationsSettingsProviderUrl,"10000001").Should().Be("https://recruit.sit-pas.apprenticeships.education.gov.uk/10000001/notifications-manage/");
        string.Format(actual.ReviewVacancyReviewInRecruitEmployerUrl,"ABC123", vacancyId).Should().Be($"https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/vacancies/{vacancyId}/check-answers/");
        string.Format(actual.ReviewVacancyReviewInRecruitProviderUrl,"10000001", vacancyId).Should().Be($"https://recruit.sit-pas.apprenticeships.education.gov.uk/10000001/vacancies/{vacancyId}/check-answers/");
        string.Format(actual.ManageAdvertUrl,"ABC123", vacancyId).Should().Be($"https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/vacancies/{vacancyId}/manage/");
    }
}