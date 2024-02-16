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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
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
        public async Task Then_SearchOrganisations(
            [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEduOrgApiClient,
            [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockRefApiClient,
            List<Organisation> refApiOrgs,
            EducationalOrganisationResponse eduOrgApiResponse,
            SearchOrganisationsQuery query,
            SearchOrganisationsQueryHandler handler,
            string testURN)
        {

            eduOrgApiResponse.EducationalOrganisations.First().URN = testURN;
            refApiOrgs.First().Code = testURN;

            mockEduOrgApiClient
                .Setup(client => client.Get<EducationalOrganisationResponse>(It.IsAny<SearchEducationalOrganisationsRequest>()))
                .ReturnsAsync(eduOrgApiResponse);

            var refApiResponse = new GetSearchOrganisationsResponse();
            refApiResponse.AddRange(refApiOrgs);

            mockRefApiClient
               .Setup(client => client.Get<GetSearchOrganisationsResponse>(It.IsAny<GetSearchOrganisationsRequest>()))
                .ReturnsAsync(refApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            var expectedTotal = eduOrgApiResponse.EducationalOrganisations.Count() + refApiResponse.Count() - 1;

            result.Organisations.Count().Should().Be(expectedTotal);
        }
    }
}
