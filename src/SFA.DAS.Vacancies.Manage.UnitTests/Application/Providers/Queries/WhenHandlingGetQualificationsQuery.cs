using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Qualifications.Queries
{
    public class WhenHandlingQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called_For_Ukprn(
            GetProviderQualificationsQueryResponse apiQueryResponse,
            GetQualificationsQuery query,
 //           [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> apiClient,
  //          GetProviderAccountLegalEntitiesQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.Get<GetQualificationsQueryResponse>(
                        It.Is<GetQualificationsRequest>(c => c.GetUrl.Contains($"ukprn={query.Ukprn}"))))
                .ReturnsAsync(apiQueryResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Qualifications.Should().BeEquivalentTo(apiQueryResponse.Qualifications);
        }
    }
}