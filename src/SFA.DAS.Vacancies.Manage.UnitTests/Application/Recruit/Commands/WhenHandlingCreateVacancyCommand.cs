using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Recruit.Commands
{
    public class WhenHandlingCreateVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Employer_And_Api_Called_With_Response(
            string responseValue,
            CreateVacancyCommand command,
            AccountDetail accountDetailApiResponse,
            List<GetEmployerAccountLegalEntityItem> legalEntities,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            legalEntities.First().AccountLegalEntityPublicHashedId =
                command.PostVacancyRequestData.AccountLegalEntityPublicHashedId;
            command.PostVacancyRequestData.OwnerType = OwnerType.Employer;
            var apiResponse = new ApiResponse<string>(responseValue, HttpStatusCode.Created, "");
            recruitApiClient.Setup(x =>
                x.PostWithResponseCode<string>(
                    It.Is<PostVacancyRequest>(c => 
                        c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail={command.PostVacancyRequestData.User.Email}")
                        && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                        && ((PostVacancyRequestData)c.Data).LegalEntityName.Equals(legalEntities.First().AccountLegalEntityName)
                        )))
                .ReturnsAsync(apiResponse);
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{command.PostVacancyRequestData.EmployerAccountId}"))))
                .ReturnsAsync(accountDetailApiResponse);
            for (var i = 0; i < accountDetailApiResponse.LegalEntities.Count; i++)
            {
                var index = i;
                accountsApi
                    .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                        It.Is<GetEmployerAccountLegalEntityRequest>(request =>
                            request.GetUrl.Equals(accountDetailApiResponse.LegalEntities[index].Href))))
                    .ReturnsAsync(legalEntities[index]);
            }

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Legal_Entity_Does_Not_Belong_To_The_Employer_Account_Exception_Thrown(
            string responseValue,
            CreateVacancyCommand command,
            AccountDetail accountDetailApiResponse,
            GetEmployerAccountLegalEntityItem leaglEntityResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            command.PostVacancyRequestData.OwnerType = OwnerType.Employer;
            accountsApi
                .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                    It.IsAny<GetEmployerAccountLegalEntityRequest>()))
                .ReturnsAsync(leaglEntityResponse);
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{command.PostVacancyRequestData.EmployerAccountId}"))))
                .ReturnsAsync(accountDetailApiResponse);
            
            //Act
            Assert.ThrowsAsync<SecurityException>(()=> handler.Handle(command, CancellationToken.None));
            
            recruitApiClient.Verify(x =>
                x.PostWithResponseCode<string>(
                    It.IsAny<PostVacancyRequest>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Legal_Entity_Does_Not_Belong_To_The_Provider_Relations_Then_Exception_Thrown(
            string responseValue,
            CreateVacancyCommand command,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.PostVacancyRequestData.OwnerType = OwnerType.Provider;
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>())).ReturnsAsync(response);
            
            //Act
            Assert.ThrowsAsync<SecurityException>(()=> handler.Handle(command, CancellationToken.None));
            
            recruitApiClient.Verify(x =>
                x.PostWithResponseCode<string>(
                    It.IsAny<PostVacancyRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Provider_And_Api_Called_With_Response(
            string responseValue,
            CreateVacancyCommand command,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<string>(responseValue, HttpStatusCode.Created, "");
            command.PostVacancyRequestData.OwnerType = OwnerType.Provider;
            response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId = command.PostVacancyRequestData.AccountLegalEntityPublicHashedId;
            recruitApiClient.Setup(x =>
                    x.PostWithResponseCode<string>(
                        It.Is<PostVacancyRequest>(c => 
                            c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail={command.PostVacancyRequestData.User.Email}")
                            && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                            && ((PostVacancyRequestData)c.Data).LegalEntityName.Equals(response.AccountProviderLegalEntities.First().AccountLegalEntityName)
                        )))
                .ReturnsAsync(apiResponse);
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetProviderAccountLegalEntitiesRequest>(c =>
                    c.GetUrl.Contains(command.PostVacancyRequestData.User.Ukprn.ToString())))).ReturnsAsync(response);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body);
        }
        
        [Test, MoqAutoData]
        public void Then_If_There_Is_A_Bad_Request_Error_From_The_Api_A_Exception_Is_Returned(
            string errorContent,
            CreateVacancyCommand command,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.PostVacancyRequestData.OwnerType = OwnerType.Provider;
            response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId = command.PostVacancyRequestData.AccountLegalEntityPublicHashedId;
            var apiResponse = new ApiResponse<string>(null, HttpStatusCode.BadRequest, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<string>(It.IsAny<PostVacancyRequest>()))
                .ReturnsAsync(apiResponse);
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>())).ReturnsAsync(response);
            
            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().Throw<HttpRequestContentException>().WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})")
                .Which.ErrorContent.Should().Be(errorContent);
        }
        [Test, MoqAutoData]
        public void Then_If_There_Is_An_Error_From_The_Api_A_Exception_Is_Returned(
            string errorContent,
            CreateVacancyCommand command,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.PostVacancyRequestData.OwnerType = OwnerType.Provider;
            response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId = command.PostVacancyRequestData.AccountLegalEntityPublicHashedId;
            var apiResponse = new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<string>(It.IsAny<PostVacancyRequest>()))
                .ReturnsAsync(apiResponse);
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>())).ReturnsAsync(response);
            
            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().Throw<Exception>()
                .WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.InternalServerError} ({HttpStatusCode.InternalServerError})");
        }
    }
}