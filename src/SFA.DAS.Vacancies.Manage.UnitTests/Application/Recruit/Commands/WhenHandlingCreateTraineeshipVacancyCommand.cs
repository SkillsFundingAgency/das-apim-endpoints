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
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Recruit.Commands
{
    public class WhenHandlingCreateTraineeshipVacancyCommand
    {

        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Provider_And_Api_Called_With_Response(
            int accountIdentifierId,
            string responseValue,
            CreateTraineeshipVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateTraineeshipVacancyCommandHandler handler)
        {
            //Arrange
            command.AccountIdentifier = new AccountIdentifier($"Provider-{accountIdentifierId}-Product");
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<string>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient.Setup(x =>
                x.PostWithResponseCode<string>(
                    It.Is<PostTraineeshipVacancyRequest>(c =>
                        c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail=")
                        && ((PostTraineeshipVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                        && ((PostTraineeshipVacancyRequestData)c.Data).LegalEntityName.Equals(accountLegalEntityItem.Name)
                        && ((PostTraineeshipVacancyRequestData)c.Data).EmployerAccountId.Equals(accountLegalEntityItem.AccountHashedId)
                        )))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyRequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body);
            mockRecruitApiClient.Verify(client => client.PostWithResponseCode<string>(It.IsAny<PostValidateVacancyRequest>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Request_Is_To_Set_The_EmployerNameOption_As_RegisteredAddress_Then_Employer_Name_Is_Set(
            int accountIdentifierId,
            string responseValue,
            CreateTraineeshipVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateTraineeshipVacancyCommandHandler handler)
        {
            //Arrange
            command.PostVacancyRequestData.EmployerNameOption = TraineeshipEmployerNameOption.RegisteredName;
            command.AccountIdentifier = new AccountIdentifier($"Provider-{accountIdentifierId}-Product");
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<string>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient.Setup(x =>
                    x.PostWithResponseCode<string>(
                        It.Is<PostTraineeshipVacancyRequest>(c =>
                            c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail=")
                            && ((PostTraineeshipVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                            && ((PostTraineeshipVacancyRequestData)c.Data).LegalEntityName.Equals(accountLegalEntityItem.Name)
                            && ((PostTraineeshipVacancyRequestData)c.Data).EmployerName.Equals(accountLegalEntityItem.Name)
                            && ((PostTraineeshipVacancyRequestData)c.Data).EmployerAccountId.Equals(accountLegalEntityItem.AccountHashedId)
                        )))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyRequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body);
        }

        [Test, MoqAutoData]
        public async Task And_IsSandbox_Then_Api_Called_To_Validate_Request(
            string responseValue,
            CreateTraineeshipVacancyCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockRecruitApiClient,
            CreateTraineeshipVacancyCommandHandler handler)
        {
            //Arrange
            command.IsSandbox = true;
            var apiResponse = new ApiResponse<string>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient
                .Setup(x =>
                    x.PostWithResponseCode<string>(
                        It.Is<PostValidateTraineeshipVacancyRequest>(c =>
                            c.PostUrl.Contains($"{command.Id.ToString()}/ValidateTraineeship?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail={command.PostVacancyRequestData.User.Email}")
                            && ((PostTraineeshipVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                        )))
                .ReturnsAsync(apiResponse);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body);
            mockRecruitApiClient.Verify(client => client.PostWithResponseCode<string>(It.IsAny<PostVacancyRequest>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public void Then_If_There_Is_A_Bad_Request_Error_From_The_Api_A_Exception_Is_Returned(
            string errorContent,
            CreateTraineeshipVacancyCommand command,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateTraineeshipVacancyCommandHandler handler)
        {
            //Arrange
            response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId = command.PostVacancyRequestData.AccountLegalEntityPublicHashedId;
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<string>(null, HttpStatusCode.BadRequest, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<string>(It.IsAny<PostTraineeshipVacancyRequest>()))
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
            CreateTraineeshipVacancyCommand command,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateTraineeshipVacancyCommandHandler handler)
        {
            //Arrange
            response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId = command.PostVacancyRequestData.AccountLegalEntityPublicHashedId;
            command.IsSandbox = false;
            var apiResponse = new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<string>(It.IsAny<PostTraineeshipVacancyRequest>()))
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