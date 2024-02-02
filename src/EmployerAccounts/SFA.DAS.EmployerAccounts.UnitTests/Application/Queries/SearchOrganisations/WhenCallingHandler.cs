using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.SearchOrganisations
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_SearchOrganisation_from_Reference_Apid(
            SearchOrganisationsQuery query,
            GetSearchOrganisationsResponse apiResponse,
            [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApiClient,
            SearchOrganisationsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetSearchOrganisationsResponse>(It.IsAny<GetSearchOrganisationsRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Organisations.Should().BeEquivalentTo(apiResponse.Organisations);
        }
    }
}
