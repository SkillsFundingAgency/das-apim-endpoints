using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.UnitTests.Domain;

public class EmailEnvironmentHelperTests
{
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Production()
    {
        var actual = new EmailEnvironmentHelper("PRd");

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("b648c047-b2e3-4ebe-b9b4-a6cc59c2af94");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("8387c857-b4f9-4cfb-8c44-df4c2560e446");
        actual.VacancyReviewApprovedTemplateId.Should().Be("d8855c4f-9ce1-4870-93ff-53e609f59a51");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
        string.Format(actual.NotificationsSettingsEmployerUrl,"ABC123").Should().Be("https://recruit.manage-apprenticeships.service.gov.uk/accounts/ABC123/notifications-manage/");
        string.Format(actual.NotificationsSettingsProviderUrl,"10000001").Should().Be("https://recruit.providers.apprenticeships.education.gov.uk/10000001/notifications-manage/");
    }
    
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Non_Production()
    {
        var actual = new EmailEnvironmentHelper("siT");

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("35e8b348-5f26-488a-8165-459522f8189b");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("95d7ff0c-79fc-4585-9fff-5e583b478d23");
        actual.VacancyReviewApprovedTemplateId.Should().Be("9a45ff1d-769d-4be2-96fb-dcf605e0108f");
        actual.CandidateApplicationUrl.Should().Be("https://sit-findapprenticeship.apprenticeships.education.gov.uk/applications");
        string.Format(actual.NotificationsSettingsEmployerUrl,"ABC123").Should().Be("https://recruit.sit-eas.apprenticeships.education.gov.uk/accounts/ABC123/notifications-manage/");
        string.Format(actual.NotificationsSettingsProviderUrl,"10000001").Should().Be("https://recruit.sit-pas.apprenticeships.education.gov.uk/10000001/notifications-manage/");
    }
}