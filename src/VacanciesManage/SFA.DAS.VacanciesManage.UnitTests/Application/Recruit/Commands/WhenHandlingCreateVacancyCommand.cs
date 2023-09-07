using System;
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
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.VacanciesManage.Configuration;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.Interfaces;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.Recruit.Commands
{
    public class WhenHandlingCreateVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Employer_And_Api_Called_With_Response(
            long responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            command.PostVacancyRequestData.OwnerType = OwnerType.Employer;
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<long?>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient.Setup(x =>
                x.PostWithResponseCode<long?>(
                    It.Is<PostVacancyRequest>(c =>
                        c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail=")
                        && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                        && ((PostVacancyRequestData)c.Data).LegalEntityName.Equals(accountLegalEntityItem.Name)
                        && ((PostVacancyRequestData)c.Data).EmployerAccountId.Equals(command.PostVacancyRequestData.EmployerAccountId)
                        ), true))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyRequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body.ToString());
            mockRecruitApiClient.Verify(client => client.PostWithResponseCode<long?>(It.IsAny<PostValidateVacancyRequest>(), true),
                Times.Never);
        }
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Provider_And_Api_Called_With_Response(
            int accountIdentifierId,
            long responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.AccountIdentifier = new AccountIdentifier($"Provider-{accountIdentifierId}-Product");
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<long?>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient.Setup(x =>
                x.PostWithResponseCode<long?>(
                    It.Is<PostVacancyRequest>(c =>
                        c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail=")
                        && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                        && ((PostVacancyRequestData)c.Data).LegalEntityName.Equals(accountLegalEntityItem.Name)
                        && ((PostVacancyRequestData)c.Data).EmployerAccountId.Equals(accountLegalEntityItem.AccountHashedId)
                        ), true))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyRequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body.ToString());
            mockRecruitApiClient.Verify(client => client.PostWithResponseCode<string>(It.IsAny<PostValidateVacancyRequest>(), true),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Request_Is_To_Set_The_EmployerNameOption_As_RegisteredAddress_Then_Employer_Name_Is_Set(
            int accountIdentifierId,
            long responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.PostVacancyRequestData.EmployerNameOption = EmployerNameOption.RegisteredName;
            command.AccountIdentifier = new AccountIdentifier($"Provider-{accountIdentifierId}-Product");
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<long?>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient.Setup(x =>
                    x.PostWithResponseCode<long?>(
                        It.Is<PostVacancyRequest>(c =>
                            c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail=")
                            && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                            && ((PostVacancyRequestData)c.Data).LegalEntityName.Equals(accountLegalEntityItem.Name)
                            && ((PostVacancyRequestData)c.Data).EmployerName.Equals(accountLegalEntityItem.Name)
                            && ((PostVacancyRequestData)c.Data).EmployerAccountId.Equals(accountLegalEntityItem.AccountHashedId)
                        ), true))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyRequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body.ToString());
        }

        [Test, MoqAutoData]
        public async Task And_IsSandbox_Then_Api_Called_To_Validate_Request(
            long responseValue,
            CreateVacancyCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.IsSandbox = true;
            var apiResponse = new ApiResponse<long?>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient
                .Setup(x =>
                    x.PostWithResponseCode<long?>(
                        It.Is<PostValidateVacancyRequest>(c =>
                            c.PostUrl.Contains($"{command.Id.ToString()}/validate?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail={command.PostVacancyRequestData.User.Email}")
                            && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                            && ((PostVacancyRequestData)c.Data).AccountType.Equals(command.PostVacancyRequestData.AccountType)
                        ), true))
                .ReturnsAsync(apiResponse);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body.ToString());
            mockRecruitApiClient.Verify(client => client.PostWithResponseCode<long?>(It.IsAny<PostVacancyRequest>(), true),
                Times.Never);
        }

        [Test, MoqAutoData]
        public void Then_If_The_Legal_Entity_Does_Not_Belong_To_The_Employer_Account_Exception_Thrown(
            string responseValue,
            CreateVacancyCommand command,
            AccountDetail accountDetailApiResponse,
            GetEmployerAccountLegalEntityItem leaglEntityResponse,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            command.PostVacancyRequestData.OwnerType = OwnerType.Employer;
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync((AccountLegalEntityItem)null);

            //Act
            Assert.ThrowsAsync<SecurityException>(() => handler.Handle(command, CancellationToken.None));

            recruitApiClient.Verify(x =>
                x.PostWithResponseCode<long?>(
                    It.IsAny<PostVacancyRequest>(), true), Times.Never);
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
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<long?>(null, HttpStatusCode.BadRequest, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<long?>(It.IsAny<PostVacancyRequest>(), true))
                .ReturnsAsync(apiResponse);
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>())).ReturnsAsync(response);

            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            act.Should().ThrowAsync<HttpRequestContentException>().WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})")
                .Result.Which.ErrorContent.Should().Be(errorContent);
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
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<long?>(null, HttpStatusCode.InternalServerError, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<long?>(It.IsAny<PostVacancyRequest>(), true))
                .ReturnsAsync(apiResponse);
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>())).ReturnsAsync(response);

            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            act.Should().ThrowAsync<Exception>()
                .WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.InternalServerError} ({HttpStatusCode.InternalServerError})");
        }
    }
}