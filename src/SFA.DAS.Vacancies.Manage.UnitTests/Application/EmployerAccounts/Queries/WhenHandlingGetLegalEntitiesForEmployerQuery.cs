using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.EmployerAccounts.Queries
{
    public class WhenHandlingGetLegalEntitiesForEmployerQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Legal_Entities_For_Account_And_Only_Adds_Those_With_A_Signed_Agreement(
            GetLegalEntitiesForEmployerQuery query,
            AccountDetail apiResponse,
            List<GetEmployerAccountLegalEntityItem> legalEntities,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockApiClient,
            GetLegalEntitiesForEmployerQueryHandler handler)
        {
            //arrange
            mockApiClient
                .Setup(client => client.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(request =>
                        request.EncodedAccountId.Equals(query.EncodedAccountId))))
                .ReturnsAsync(apiResponse);

            legalEntities.First().Agreements.First().Status = EmployerAgreementStatus.Signed;
            for (var i = 0; i < apiResponse.LegalEntities.Count; i++)
            {
                var index = i;
                mockApiClient
                    .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                        It.Is<GetEmployerAccountLegalEntityRequest>(request =>
                            request.GetUrl.Equals(apiResponse.LegalEntities[index].Href))))
                    .ReturnsAsync(legalEntities[index]);
            }

            //act
            var result = await handler.Handle(query, CancellationToken.None);

            //assert
            result.LegalEntities.Should()
                .BeEquivalentTo(legalEntities.Where(c=>
                    c.Agreements.Any(x=>x.Status == EmployerAgreementStatus.Signed)).ToList(), options=>options.Excluding(c=>c.AccountPublicHashedId));
            result.LegalEntities.ToList().TrueForAll(c => c.AccountPublicHashedId.Equals(query.EncodedAccountId))
                .Should().BeTrue();
            result.LegalEntities.ToList().TrueForAll(c => c.AccountName.Equals(apiResponse.DasAccountName))
                .Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Account_Not_Found_Empty_Response_Returned(
            GetLegalEntitiesForEmployerQuery query,
            AccountDetail apiResponse,
            List<GetEmployerAccountLegalEntityItem> legalEntities,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockApiClient,
            GetLegalEntitiesForEmployerQueryHandler handler)
        {
            //arrange
            mockApiClient
                .Setup(client => client.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(request =>
                        request.EncodedAccountId.Equals(query.EncodedAccountId))))
                .ReturnsAsync((AccountDetail)null);
            
            //act
            var result = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            result.LegalEntities.Should().BeEmpty();
        }
    }
}