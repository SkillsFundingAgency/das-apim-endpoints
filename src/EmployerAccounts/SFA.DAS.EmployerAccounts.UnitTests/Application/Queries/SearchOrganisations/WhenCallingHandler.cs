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
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.SearchOrganisations
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_SearchOrganisation_from_Reference_Api(
            SearchOrganisationsQuery query,
            EducationalOrganisationResponse apiResponse,
            [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockApiClient,
            SearchOrganisationsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<EducationalOrganisationResponse>(It.IsAny<SearchEducationalOrganisationsRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Organisations.Count().Should().Be(apiResponse.EducationalOrganisations.Count());

            result.Organisations.First().Name.Should().Be(apiResponse.EducationalOrganisations.First().Name);
            result.Organisations.First().Type.Should().Be(OrganisationType.EducationOrganisation);
            result.Organisations.First().SubType.Should().Be(OrganisationSubType.None);
            result.Organisations.First().Code.Should().Be(apiResponse.EducationalOrganisations.First().URN);
            result.Organisations.First().Sector.Should().Be(apiResponse.EducationalOrganisations.First().EducationalType);

            result.Organisations.First().Address.Line1.Should().Be(apiResponse.EducationalOrganisations.First().AddressLine1);
            result.Organisations.First().Address.Line2.Should().Be(apiResponse.EducationalOrganisations.First().AddressLine2);
            result.Organisations.First().Address.Line3.Should().Be(apiResponse.EducationalOrganisations.First().AddressLine3);
            result.Organisations.First().Address.Line4.Should().Be(apiResponse.EducationalOrganisations.First().Town);
            result.Organisations.First().Address.Line5.Should().Be(apiResponse.EducationalOrganisations.First().County);
            result.Organisations.First().Address.Postcode.Should().Be(apiResponse.EducationalOrganisations.First().PostCode);
        }
    }
}
