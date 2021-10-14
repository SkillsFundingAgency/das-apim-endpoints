using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.EmployerAccounts.Queries
{
    public class WhenHandlingGetLegalEntitiesForEmployerQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Legal_Entities_For_Account(
            GetLegalEntitiesForEmployerQuery query,
            List<Resource> apiResponse,
            List<GetEmployerAccountLegalEntityItem> legalEntities,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockApiClient,
            GetLegalEntitiesForEmployerQueryHandler handler)
        {
            //arrange
            mockApiClient
                .Setup(client => client.GetAll<Resource>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(request =>
                        request.EncodedAccountId.Equals(query.EncodedAccountId))))
                .ReturnsAsync(apiResponse);

            for (var i = 0; i < apiResponse.Count; i++)
            {
                var index = i;
                mockApiClient
                    .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                        It.Is<GetEmployerAccountLegalEntityRequest>(request =>
                            request.GetUrl.Equals(apiResponse[index].Href))))
                    .ReturnsAsync(legalEntities[index]);
            }
            
            //act
            var result = await handler.Handle(query, CancellationToken.None);

            //assert
            result.LegalEntities.Should().BeEquivalentTo(legalEntities);
        }
    }
}