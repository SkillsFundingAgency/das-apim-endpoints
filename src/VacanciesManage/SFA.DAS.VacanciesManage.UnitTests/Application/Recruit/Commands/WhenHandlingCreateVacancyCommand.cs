using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.Roatp;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.VacanciesManage.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading;
using HttpRequestContentException = SFA.DAS.Apim.Shared.Infrastructure.HttpRequestContentException;
using Vacancy = SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.Recruit.Commands;

public class WhenHandlingCreateVacancyCommand
{
    private const int ExpectedLarsCode = 123;

    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Employer_And_Api_Called_With_Response(
        Vacancy responseValue,
        CreateVacancyCommand command,
        AccountLegalEntityItem accountLegalEntityItem,
        ProviderDetailsModel trainingProviderDetails,
        [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
        [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
        [Frozen] Mock<ICourseService> courseServiceMock,
        [Frozen] Mock<ITrainingProviderService> trainingProviderService,
        CreateVacancyCommandHandler handler)
    {
        //Arrange
        command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
        command.PostVacancyRequest.OwnerType = OwnerType.Employer;
        command.IsSandbox = false;
        command.PostVacancyRequest.ProgrammeId = ExpectedLarsCode.ToString();

        var matchingStandard = new GetStandardsListItem
        {
            LarsCode = ExpectedLarsCode,
            ApprenticeshipType = ApprenticeshipType.Apprenticeship
        };

        var getStandardsResponse = new GetStandardsListResponse
        {
            Standards = new List<GetStandardsListItem> { matchingStandard }
        };

        trainingProviderService
            .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
            .ReturnsAsync(trainingProviderDetails);

        courseServiceMock
            .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(getStandardsResponse);

        var apiResponse = new Apim.Shared.Models.ApiResponse<Vacancy>(responseValue, HttpStatusCode.Created, "");
        mockRecruitApiClient
            .Setup(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>(), true))
            .ReturnsAsync(apiResponse);
        accountLegalEntityPermissionService
            .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                command.PostVacancyRequest.AccountLegalEntityPublicHashedId))
            .ReturnsAsync(accountLegalEntityItem);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.VacancyReference.Should().Be(apiResponse.Body.VacancyReference.ToString());

        mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Once);
        mockRecruitApiClient.Verify(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public Task Then_The_Command_Is_Handled_When_Provider_Is_Null_Throws_Exception(
        Vacancy responseValue,
        CreateVacancyCommand command,
        AccountLegalEntityItem accountLegalEntityItem,
        ProviderDetailsModel trainingProviderDetails,
        [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
        [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
        [Frozen] Mock<ICourseService> courseServiceMock,
        [Frozen] Mock<ITrainingProviderService> trainingProviderService,
        CreateVacancyCommandHandler handler)
    {
        //Arrange
        command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
        command.PostVacancyRequest.OwnerType = OwnerType.Employer;
        command.IsSandbox = false;
        command.PostVacancyRequest.ProgrammeId = ExpectedLarsCode.ToString();

        var matchingStandard = new GetStandardsListItem
        {
            LarsCode = ExpectedLarsCode,
            ApprenticeshipType = ApprenticeshipType.Apprenticeship
        };

        var getStandardsResponse = new GetStandardsListResponse
        {
            Standards = new List<GetStandardsListItem> { matchingStandard }
        };

        trainingProviderService
            .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
            .ReturnsAsync((ProviderDetailsModel)null!);

        courseServiceMock
            .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(getStandardsResponse);

        var apiResponse = new Apim.Shared.Models.ApiResponse<Vacancy>(responseValue, HttpStatusCode.Created, "");
        mockRecruitApiClient
            .Setup(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>(), true))
            .ReturnsAsync(apiResponse);

        accountLegalEntityPermissionService
            .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                command.PostVacancyRequest.AccountLegalEntityPublicHashedId))
            .ReturnsAsync(accountLegalEntityItem);

        //Act
        Assert.ThrowsAsync<HttpRequestContentException>(() => handler.Handle(command, CancellationToken.None));

        mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Never);
        mockRecruitApiClient.Verify(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()), Times.Never);
        return Task.CompletedTask;
    }

    public class CreateVacancyCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Then_Throws_SecurityException_When_AccountLegalEntity_Is_Null(
            CreateVacancyCommand command,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync((AccountLegalEntityItem)null!);

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<SecurityException>();
            mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Never);
            mockRecruitApiClient.Verify(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Throws_When_TrainingProvider_Is_Null(
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync(accountLegalEntityItem);

            trainingProviderService
                .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                .ReturnsAsync((ProviderDetailsModel)null!);

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Training Provider UKPRN not valid");
            mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Never);
            mockRecruitApiClient.Verify(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Throws_When_TrainingProvider_Is_Not_Main_Or_Employer_Profile(
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            trainingProviderDetails.ProviderType = ProviderType.Supporting;

            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync(accountLegalEntityItem);

            trainingProviderService
                .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                .ReturnsAsync(trainingProviderDetails);

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("UKPRN of a training provider must be registered to deliver apprenticeship training");
            mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Never);
            mockRecruitApiClient.Verify(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Throws_When_Course_StartDate_Exceeds_LastDateStarts(
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<ICourseService> courseServiceMock,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            trainingProviderDetails.ProviderType = ProviderType.Main;
            command.PostVacancyRequest.ProgrammeId = ExpectedLarsCode.ToString();
            command.PostVacancyRequest.StartDate = DateTime.UtcNow.AddMonths(6);

            var matchingStandard = new GetStandardsListItem
            {
                LarsCode = ExpectedLarsCode,
                ApprenticeshipType = ApprenticeshipType.Apprenticeship,
                LastDateStarts = DateTime.UtcNow.AddMonths(3)
            };

            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync(accountLegalEntityItem);

            trainingProviderService
                .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                .ReturnsAsync(trainingProviderDetails);

            courseServiceMock
                .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(new GetStandardsListResponse { Standards = [matchingStandard] });

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<HttpRequestContentException>();
            mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Never);
            mockRecruitApiClient.Verify(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Sets_Status_To_Review_When_Employer_Approval_Required(
            Vacancy responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            VacancyReview putVacancyReviewResponse,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            [Frozen] Mock<ICourseService> courseServiceMock,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            command.AccountIdentifier = new AccountIdentifier("Provider-ABC123-Product");
            command.PostVacancyRequest.OwnerType = OwnerType.Provider;
            command.PostVacancyRequest.ProgrammeId = ExpectedLarsCode.ToString();

            var matchingStandard = new GetStandardsListItem
            {
                LarsCode = ExpectedLarsCode,
                ApprenticeshipType = ApprenticeshipType.Apprenticeship
            };

            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync(accountLegalEntityItem);

            accountLegalEntityPermissionService
                .Setup(x => x.HasProviderGotEmployersPermissionAsync(
                    It.IsAny<int>(),
                    It.IsAny<long>(),
                    It.IsAny<List<SFA.DAS.SharedOuterApi.Models.ProviderRelationships.Operation>>()))
                .ReturnsAsync(false);

            trainingProviderService
                .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                .ReturnsAsync(trainingProviderDetails);

            courseServiceMock
                .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(new GetStandardsListResponse { Standards = [matchingStandard] });

            var apiResponse = new Apim.Shared.Models.ApiResponse<Vacancy>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient
                .Setup(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            mockRecruitApiClient
                .Setup(x => x.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(It.IsAny<PutVacancyreviewsByIdApiRequest>()))
                .ReturnsAsync(new Apim.Shared.Models.ApiResponse<VacancyReview>(putVacancyReviewResponse, HttpStatusCode.OK, ""));

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            command.PostVacancyRequest.Status.Should().Be(VacancyStatus.Review);
            mockRecruitApiClient.Verify(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Sets_Status_To_Submitted_When_Employer_Approval_Not_Required(
            Vacancy responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            [Frozen] Mock<ICourseService> courseServiceMock,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            command.PostVacancyRequest.OwnerType = OwnerType.Employer;
            const int expectedLarsCode = 123;
            command.PostVacancyRequest.ProgrammeId = ExpectedLarsCode.ToString();

            var matchingStandard = new GetStandardsListItem
            {
                LarsCode = ExpectedLarsCode,
                ApprenticeshipType = ApprenticeshipType.Apprenticeship
            };

            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync(accountLegalEntityItem);

            trainingProviderService
                .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                .ReturnsAsync(trainingProviderDetails);

            courseServiceMock
                .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(new GetStandardsListResponse { Standards = [matchingStandard] });

            var apiResponse = new Apim.Shared.Models.ApiResponse<Vacancy>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient
                .Setup(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            command.PostVacancyRequest.Status.Should().Be(VacancyStatus.Submitted);
            command.PostVacancyRequest.SubmittedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Test, MoqAutoData]
        public async Task Then_Clears_Qualifications_And_Skills_For_Foundation_Apprenticeship(
            Vacancy responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<DAS.Recruit.Contracts.Client.IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            [Frozen] Mock<ICourseService> courseServiceMock,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            CreateVacancyCommandHandler handler)
        {
            // Arrange
            trainingProviderDetails.ProviderType = ProviderType.Employer;
            command.PostVacancyRequest.ProgrammeId = ExpectedLarsCode.ToString();

            var matchingStandard = new GetStandardsListItem
            {
                LarsCode = ExpectedLarsCode,
                ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship
            };

            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                .ReturnsAsync(accountLegalEntityItem);

            trainingProviderService
                .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                .ReturnsAsync(trainingProviderDetails);

            courseServiceMock
                .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(new GetStandardsListResponse { Standards = [matchingStandard] });

            var apiResponse = new Apim.Shared.Models.ApiResponse<Vacancy>(responseValue, HttpStatusCode.Created, "");
            mockRecruitApiClient
                .Setup(x => x.PostWithResponseCode<Vacancy>(It.IsAny<PostVacanciesApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            command.PostVacancyRequest.Qualifications.Should().BeEmpty();
            command.PostVacancyRequest.Skills.Should().BeEmpty();
            command.PostVacancyRequest.ApprenticeshipType.Should().Be(ApprenticeshipTypes.Foundation);
        }
    }
}