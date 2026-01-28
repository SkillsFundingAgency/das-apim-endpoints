using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProvider;

public sealed class WhenMappingGetCourseProviderQueryResultFromModel
{
    [Test]
    [MoqAutoData]
    public void Then_All_Properties_Must_Map_Correctly(GetCourseProviderDetailsResponse response)
    {
        GetCourseProviderQueryResult sut = response;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Ukprn, Is.EqualTo(response.Ukprn));
            Assert.That(sut.ProviderName, Is.EqualTo(response.ProviderName));
            Assert.That(sut.ProviderAddress.AddressLine1, Is.EquivalentTo(response.Address.AddressLine1));
            Assert.That(sut.ProviderAddress.AddressLine2, Is.EquivalentTo(response.Address.AddressLine2));
            Assert.That(sut.ProviderAddress.AddressLine3, Is.EquivalentTo(response.Address.AddressLine3));
            Assert.That(sut.ProviderAddress.AddressLine4, Is.EquivalentTo(response.Address.AddressLine4));
            Assert.That(sut.ProviderAddress.Town, Is.EquivalentTo(response.Address.Town));
            Assert.That(sut.ProviderAddress.Postcode, Is.EquivalentTo(response.Address.Postcode));
            Assert.That(sut.Contact.MarketingInfo, Is.EquivalentTo(response.Contact.MarketingInfo));
            Assert.That(sut.Contact.Email, Is.EquivalentTo(response.Contact.Email));
            Assert.That(sut.Contact.Website, Is.EquivalentTo(response.Contact.Website));
            Assert.That(sut.Contact.PhoneNumber, Is.EquivalentTo(response.Contact.PhoneNumber));
            Assert.That(sut.CourseName, Is.EqualTo(response.CourseName));
            Assert.That(sut.Level, Is.EqualTo(response.Level));
            Assert.That(sut.LarsCode, Is.EqualTo(response.LarsCode.ToString()));
            Assert.That(sut.IFateReferenceNumber, Is.EqualTo(response.IFateReferenceNumber));
            Assert.That(sut.Qar.Period, Is.EqualTo(response.QAR.Period));
            Assert.That(sut.Qar.Leavers, Is.EqualTo(response.QAR.Leavers));
            Assert.That(sut.Qar.AchievementRate, Is.EqualTo(response.QAR.AchievementRate));
            Assert.That(sut.Qar.NationalLeavers, Is.EqualTo(response.QAR.NationalLeavers));
            Assert.That(sut.Qar.NationalAchievementRate, Is.EqualTo(response.QAR.NationalAchievementRate));
            Assert.That(sut.Reviews.ReviewPeriod, Is.EqualTo(response.Reviews.ReviewPeriod));
            Assert.That(sut.Reviews.EmployerReviews, Is.EqualTo(response.Reviews.EmployerReviews));
            Assert.That(sut.Reviews.EmployerStars, Is.EqualTo(response.Reviews.EmployerStars));
            Assert.That(sut.Reviews.EmployerRating, Is.EqualTo(response.Reviews.EmployerRating));
            Assert.That(sut.Reviews.ApprenticeReviews, Is.EqualTo(response.Reviews.ApprenticeReviews));
            Assert.That(sut.Reviews.ApprenticeStars, Is.EqualTo(response.Reviews.ApprenticeStars));
            Assert.That(sut.Reviews.ApprenticeRating, Is.EqualTo(response.Reviews.ApprenticeRating));
            Assert.That(sut.ShortlistId, Is.EqualTo(response.ShortlistId));
            Assert.That(sut.Locations, Is.EquivalentTo(response.Locations));
        });
    }
}
