using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Responses;

public sealed class StandardModelTests
{
    [Test, MoqAutoData]
    public void CreateFrom_MapsGetStandardsListItem_ToStandardModel(
        GetStandardsListItem standardListItem,
        int order,
        int providerCount,
        int totalProvidersCount
    )
    {
        var sut = StandardModel.CreateFrom(
            standardListItem,
            order,
            providerCount,
            totalProvidersCount
        );

        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Ordering, Is.EqualTo(order));
            Assert.That(sut.ProvidersCount, Is.EqualTo(providerCount));
            Assert.That(sut.TotalProvidersCount, Is.EqualTo(totalProvidersCount));
            Assert.That(sut.StandardUId, Is.EqualTo(standardListItem.StandardUId));
            Assert.That(sut.IfateReferenceNumber, Is.EqualTo(standardListItem.IfateReferenceNumber));
            Assert.That(sut.LarsCode, Is.EqualTo(standardListItem.LarsCode.ToString()));
            Assert.That(sut.SearchScore, Is.EqualTo(standardListItem.SearchScore));
            Assert.That(sut.Title, Is.EqualTo(standardListItem.Title));
            Assert.That(sut.Level, Is.EqualTo(standardListItem.Level));
            Assert.That(sut.OverviewOfRole, Is.EqualTo(standardListItem.OverviewOfRole));
            Assert.That(sut.Keywords, Is.EqualTo(standardListItem.Keywords));
            Assert.That(sut.Route, Is.EqualTo(standardListItem.Route));
            Assert.That(sut.RouteCode, Is.EqualTo(standardListItem.RouteCode));
            Assert.That(sut.MaxFunding, Is.EqualTo(standardListItem.MaxFunding));
            Assert.That(sut.TypicalDuration, Is.EqualTo(standardListItem.TypicalDuration));
        });
    }
}