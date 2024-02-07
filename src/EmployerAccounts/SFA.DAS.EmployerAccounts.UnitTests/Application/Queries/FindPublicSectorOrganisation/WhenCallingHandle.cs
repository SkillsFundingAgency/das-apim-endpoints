using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.FindPublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.FindPublicSectorOrganisation
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_FindPublicSectorOrganisation_from_Reference_Api(
        FindPublicSectorOrganisationQuery query,
        PagedResponse<GetPublicSectorOrganisationsResponse> response,
        [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApiClient,
        FindPublicSectorOrganisationQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.GetPaged<GetPublicSectorOrganisationsResponse>(It.IsAny<GetPublicSectorOrganisationsRequest>()))
                .ReturnsAsync(response);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(response.Data);
            result.Page.Should().Be(response.Page);
            result.TotalPages.Should().Be(response.TotalPages);
        }
    }
}
