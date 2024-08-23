using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;
using SFA.DAS.EmployerAccounts.Configuration;
using SFA.DAS.EmployerAccounts.ExternalApi;
using SFA.DAS.EmployerAccounts.ExternalApi.Requests;
using SFA.DAS.EmployerAccounts.ExternalApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Charities;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Charities;
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
        [Frozen] Mock<ICompaniesHouseApiClient<CompaniesHouseApiConfiguration>> mockCompaniesHouseApi,
        GetSearchOrganisationsResponse refApiOrgs,
        EducationalOrganisationResponse eduOrgApiResponse,
        PublicSectorOrganisationsResponse psOrgApiResponse,
        SearchCompaniesResponse searchCompaniesResponse,
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

        mockCompaniesHouseApi
            .Setup(client => client.Get<SearchCompaniesResponse>(It.Is<SearchCompanyInformationRequest>(p =>
                p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(searchCompaniesResponse);


        var totalEducationResultsExpected = eduOrgApiResponse.EducationalOrganisations.Count;
        var totalPublicSectorResultsExpected = psOrgApiResponse.PublicSectorOrganisations.Count;
        var totalCompaniesResultsExpected = searchCompaniesResponse.Companies.Count();
        var totalOldReferenceDataExpected = refApiOrgs.Count(o =>
            o.Type != OrganisationType.PublicSector && o.Type != OrganisationType.EducationOrganisation && o.Type != OrganisationType.Company);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Count.Should().Be(totalEducationResultsExpected + totalPublicSectorResultsExpected +
                                               totalCompaniesResultsExpected + totalOldReferenceDataExpected);
        result.Organisations.Count(o => o.Type == OrganisationType.EducationOrganisation).Should().Be(totalEducationResultsExpected);
        result.Organisations.Count(o => o.Type == OrganisationType.PublicSector).Should().Be(totalPublicSectorResultsExpected);
        result.Organisations.Count(o => o.Type == OrganisationType.Company).Should().Be(totalCompaniesResultsExpected);
    }

    [Test, MoqAutoData]
    public async Task Then_should_return_top_2(
    [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
    [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
    [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
    [Frozen] Mock<ICompaniesHouseApiClient<CompaniesHouseApiConfiguration>> mockCompaniesHouseApi,
    [Frozen] Mock<ICharitiesApiClient<CharitiesApiConfiguration>> mockCharitiesApiClient,
    GetSearchOrganisationsResponse refApiOrgs,
    EducationalOrganisationResponse eduOrgApiResponse,
    PublicSectorOrganisationsResponse psOrgApiResponse,
    SearchCompaniesResponse searchCompaniesResponse,
    SearchCharitiesResponse charitiesResponse,
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

        mockCompaniesHouseApi
            .Setup(client => client.Get<SearchCompaniesResponse>(It.Is<SearchCompanyInformationRequest>(p =>
                p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(searchCompaniesResponse);

        mockCharitiesApiClient
            .Setup(client => client.Get<SearchCharitiesResponse>(It.Is<SearchCharitiesRequest>(p =>
                p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(charitiesResponse);


        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Count.Should().Be(query.MaximumResults);
    }

    [Test, MoqAutoData]
    public async Task Then_Company_Number_Should_Return_One_Company(
    [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
    [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
    [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
    [Frozen] Mock<ICompaniesHouseApiClient<CompaniesHouseApiConfiguration>> mockCompaniesHouseApi,
    GetSearchOrganisationsResponse refApiOrgs,
    GetCompanyInfoResponse companyInfoResponse,
    EducationalOrganisationResponse eduOrgApiResponse,
    PublicSectorOrganisationsResponse psOrgApiResponse,
    SearchOrganisationsQueryHandler handler)
    {
        var query = new SearchOrganisationsQuery
        {
            SearchTerm = "AB123456",
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

        mockCompaniesHouseApi
            .Setup(client => client.Get<GetCompanyInfoResponse>(It.Is<GetCompanyInformationRequest>(p =>
                p.Id == query.SearchTerm)))
            .ReturnsAsync(companyInfoResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Where(x => x.Type == OrganisationType.Company).Count().Should().Be(1);
    }


    [Test, MoqAutoData]
    public async Task Then_StringSearchTerm_Should_Search_For_Multiple_Companies(
    [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
    [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
    [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
    [Frozen] Mock<ICompaniesHouseApiClient<CompaniesHouseApiConfiguration>> mockCompaniesHouseApi,
    GetSearchOrganisationsResponse refApiOrgs,
    EducationalOrganisationResponse eduOrgApiResponse,
    PublicSectorOrganisationsResponse psOrgApiResponse,
    SearchCompaniesResponse searchCompaniesResponse,
    SearchOrganisationsQueryHandler handler)
    {
        var query = new SearchOrganisationsQuery
        {
            SearchTerm = "Organisation Name",
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

        mockCompaniesHouseApi
           .Setup(client => client.Get<SearchCompaniesResponse>(It.Is<SearchCompanyInformationRequest>(p =>
               p.SearchTerm == query.SearchTerm.ToUpper()
               && p.MaximumResults == query.MaximumResults)))
           .ReturnsAsync(searchCompaniesResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Where(x => x.Type == OrganisationType.Company).Count().Should().Be(searchCompaniesResponse.Companies.Count());
    }

    [Test, MoqAutoData]
    public async Task Then_RegistrationNumber_Should_Return_One_Charity(
    [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
    [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
    [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
    [Frozen] Mock<ICharitiesApiClient<CharitiesApiConfiguration>> mockCharitiesApiClient,
    [Frozen] Mock<ICompaniesHouseApiClient<CompaniesHouseApiConfiguration>> mockCompaniesHouseApi,
    GetSearchOrganisationsResponse refApiOrgs,
    EducationalOrganisationResponse eduOrgApiResponse,
    SearchCompaniesResponse searchCompaniesResponse,
    PublicSectorOrganisationsResponse psOrgApiResponse,
    GetCharityResponse charitiesResponse,
    SearchOrganisationsQueryHandler handler)
    {
        var query = new SearchOrganisationsQuery
        {
            SearchTerm = "123",
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

        mockCharitiesApiClient
            .Setup(client => client.Get<GetCharityResponse>(It.Is<GetCharityByRegistrationNumberRequest>(p =>
                p.RegistrationNumber == int.Parse(query.SearchTerm))))
            .ReturnsAsync(charitiesResponse);

        mockCompaniesHouseApi
          .Setup(client => client.Get<SearchCompaniesResponse>(It.Is<SearchCompanyInformationRequest>(p =>
              p.SearchTerm == query.SearchTerm.ToUpper()
              && p.MaximumResults == query.MaximumResults)))
          .ReturnsAsync(searchCompaniesResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Where(x => x.Type == OrganisationType.Charity).Count().Should().Be(1);
    }


    [Test, MoqAutoData]
    public async Task Then_StringSearchTerm_Should_Search_For_Multiple_Charities(
    [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
    [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSOrgApiClient,
    [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
    [Frozen] Mock<ICharitiesApiClient<CharitiesApiConfiguration>> mockCharitiesApiClient,
    [Frozen] Mock<ICompaniesHouseApiClient<CompaniesHouseApiConfiguration>> mockCompaniesHouseApi,
    GetSearchOrganisationsResponse refApiOrgs,
    EducationalOrganisationResponse eduOrgApiResponse,
    PublicSectorOrganisationsResponse psOrgApiResponse,
    SearchCharitiesResponse charitiesResponse,
    SearchCompaniesResponse searchCompaniesResponse,
    SearchOrganisationsQueryHandler handler)
    {
        var query = new SearchOrganisationsQuery
        {
            SearchTerm = "Organisation Name",
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

        mockCharitiesApiClient
            .Setup(client => client.Get<SearchCharitiesResponse>(It.Is<SearchCharitiesRequest>(p =>
                p.SearchTerm == query.SearchTerm && p.MaximumResults == query.MaximumResults)))
            .ReturnsAsync(charitiesResponse);

        mockCompaniesHouseApi
        .Setup(client => client.Get<SearchCompaniesResponse>(It.Is<SearchCompanyInformationRequest>(p =>
            p.SearchTerm == query.SearchTerm.ToUpper()
            && p.MaximumResults == query.MaximumResults)))
        .ReturnsAsync(searchCompaniesResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Organisations.Where(x => x.Type == OrganisationType.Charity).Count().Should().Be(charitiesResponse.Count);
    }
}
