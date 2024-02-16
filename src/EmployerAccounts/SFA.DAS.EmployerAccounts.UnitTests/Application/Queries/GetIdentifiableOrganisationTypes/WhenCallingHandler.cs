using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetIdentifiableOrganisationTypes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetIdentifiableOrganisationTypes
{
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_GetIdentifiableOrganisationTypes_from_Reference_Api(
          GetIdentifiableOrganisationTypesQuery query,
          string[] apiResponse,
          [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockApiClient,
          GetIdentifiableOrganisationTypesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<string[]>(It.IsAny<IdentifiableOrganisationTypesRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.OrganisationTypes.Should().BeEquivalentTo(apiResponse);
        }
    }
}
