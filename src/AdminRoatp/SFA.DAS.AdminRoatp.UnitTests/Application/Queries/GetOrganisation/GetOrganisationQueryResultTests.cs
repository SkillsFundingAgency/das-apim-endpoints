using AutoFixture.NUnit3;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisation;

public class GetOrganisationQueryResultTests
{
    [Test, AutoData]
    public void GetOrganisationQueryResult_ImplicitOperator_MapsPropertiesCorrectly(OrganisationResponse source)
    {
        GetOrganisationQueryResult result = source;
        // Assert
        Assert.That(source.OrganisationId, Is.EqualTo(result.OrganisationId));
        Assert.That(source.Ukprn, Is.EqualTo(result.Ukprn));
        Assert.That(source.LegalName, Is.EqualTo(result.LegalName));
        Assert.That(source.TradingName, Is.EqualTo(result.TradingName));
        Assert.That(source.CompanyNumber, Is.EqualTo(result.CompanyNumber));
        Assert.That(source.CharityNumber, Is.EqualTo(result.CharityNumber));
        Assert.That(source.ProviderType, Is.EqualTo(result.ProviderType));
        Assert.That(source.OrganisationTypeId, Is.EqualTo(result.OrganisationTypeId));
        Assert.That(source.OrganisationType, Is.EqualTo(result.OrganisationType));
        Assert.That(source.Status, Is.EqualTo(result.Status));
        Assert.That(source.ApplicationDeterminedDate, Is.EqualTo(result.ApplicationDeterminedDate));
        Assert.That(source.RemovedReasonId, Is.EqualTo(result.RemovedReasonId));
        Assert.That(source.RemovedReason, Is.EqualTo(result.RemovedReason));
        Assert.That(source.StartDate, Is.EqualTo(result.StartDate));
        Assert.That(source.LastUpdatedDate, Is.EqualTo(result.LastUpdatedDate));
        Assert.That(source.AllowedCourseTypes, Is.EqualTo(result.AllowedCourseTypes));
    }
}
