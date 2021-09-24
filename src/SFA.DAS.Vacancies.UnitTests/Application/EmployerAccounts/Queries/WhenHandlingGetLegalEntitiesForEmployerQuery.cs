using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.UnitTests.Application.EmployerAccounts.Queries
{
    public class WhenHandlingGetLegalEntitiesForEmployerQuery
    {
        [Test, MoqAutoData, Ignore("todo")]
        public async Task Then_Gets_Legal_Entities_For_Account(
            GetLegalEntitiesForEmployerQuery query,
            GetResourceListResponse apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockApiClient,
            GetLegalEntitiesForEmployerQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetResourceListResponse>(It.Is<GetEmployerAccountLegalEntitiesRequest>(request=>request.EncodedAccountId.Equals(query.EncodedAccountId))))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            //result.LegalEntities.Should().BeEquivalentTo(apiResponse);
        }
    }
}