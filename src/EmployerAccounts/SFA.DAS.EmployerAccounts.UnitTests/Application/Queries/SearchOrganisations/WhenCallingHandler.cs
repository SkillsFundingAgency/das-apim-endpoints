using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.SearchOrganisations;

[TestFixture]
public class WhenCallingHandler
{
    [Test, MoqAutoData]
    public async Task Then_should_replace_education_and_public_section_organisations_from_new_endpoints_and_ignore_ones_in_reference_data(
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
        [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
        GetSearchOrganisationsResponse refApiOrgs,
        EducationalOrganisationResponse eduOrgApiResponse,
        PublicSectorOrganisationsResponse psOrgApiResponse,
        SearchOrganisationsQueryHandler handler)
    {
        var query = new SearchOrganisationsQuery
        {
            SearchTerm = "XXX",
            MaximumResults = 100
        };

        mockEduOrgApiClient
            .Setup(client => client.Get<EducationalOrganisationResponse>(
                It.Is<SearchEducationalOrganisationsRequest>(p =>
                    p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(eduOrgApiResponse);

        mockPSOrgApiClient
            .Setup(client =>
                client.Get<PublicSectorOrganisationsResponse>(
                    It.Is<SearchPublicSectorOrganisationsRequest>(p => p.SearchTerm == query.SearchTerm)))
            .ReturnsAsync(psOrgApiResponse);

        mockRefApiClient
            .Setup(client => client.Get<GetSearchOrganisationsResponse>(It.Is<GetSearchOrganisationsRequest>(p =>
                p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(refApiOrgs);

        var totalEducationResultsExpected = eduOrgApiResponse.EducationalOrganisations.Count;
        var totalPublicSectorResultsExpected = psOrgApiResponse.PublicSectorOrganisations.Count;
        var totalOldReferenceDataExpected = refApiOrgs.Count(o =>
            o.Type != OrganisationType.PublicSector && o.Type != OrganisationType.EducationOrganisation);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Count.Should().Be(totalEducationResultsExpected+ totalPublicSectorResultsExpected+ totalOldReferenceDataExpected);
        result.Organisations.Count(o => o.Type == OrganisationType.EducationOrganisation).Should().Be(totalEducationResultsExpected);
        result.Organisations.Count(o => o.Type == OrganisationType.PublicSector).Should().Be(totalPublicSectorResultsExpected);
    }

    [Test, MoqAutoData]
    public async Task Then_should_return_top_2(
    [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
    [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
    [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
    GetSearchOrganisationsResponse refApiOrgs,
    EducationalOrganisationResponse eduOrgApiResponse,
    PublicSectorOrganisationsResponse psOrgApiResponse,
    SearchOrganisationsQueryHandler handler)
    {
        var query = new SearchOrganisationsQuery
        {
            SearchTerm = "XXX",
            MaximumResults = 2
        };

        mockEduOrgApiClient
            .Setup(client => client.Get<EducationalOrganisationResponse>(
                It.Is<SearchEducationalOrganisationsRequest>(p =>
                    p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(eduOrgApiResponse);

        mockPSOrgApiClient
            .Setup(client =>
                client.Get<PublicSectorOrganisationsResponse>(
                    It.Is<SearchPublicSectorOrganisationsRequest>(p => p.SearchTerm == query.SearchTerm)))
            .ReturnsAsync(psOrgApiResponse);

        mockRefApiClient
            .Setup(client => client.Get<GetSearchOrganisationsResponse>(It.Is<GetSearchOrganisationsRequest>(p =>
                p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(refApiOrgs);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Count.Should().Be(query.MaximumResults);
    }
}