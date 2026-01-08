using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByFilterRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly_With_ReviewStatus(List<string> status)
    {
        var actual = new GetVacancyReviewsByFilterRequest(status);
        
        actual.GetUrl.Should().Be($"api/vacancyreviews?reviewStatus={string.Join("&reviewStatus=",status)}&expiredAssignationDateTime=");
    }
    
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly_With_ExpiredAssignationDateTime(DateTime expiredAssignationDateTime)
    {
        var actual = new GetVacancyReviewsByFilterRequest(expiredAssignationDateTime:expiredAssignationDateTime);
        
        actual.GetUrl.Should().Be($"api/vacancyreviews?reviewStatus=&expiredAssignationDateTime={expiredAssignationDateTime}");
    }
}