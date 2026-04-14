using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Models.Roatp;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading;
using OwnerType = SFA.DAS.VacanciesManage.InnerApi.Requests.OwnerType;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.Recruit.Commands
{
    public class WhenHandlingCreateVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_With_Account_Info_looked_Up_For_Employer_And_Api_Called_With_Response(
            PostVacancyResponse responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            [Frozen] Mock<ICourseService> courseServiceMock,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            command.PostVacancyV2RequestData.OwnerType = OwnerType.Employer;
            command.IsSandbox = false;
            var expectedLarsCode = 123;
            command.PostVacancyV2RequestData.ProgrammeId = expectedLarsCode.ToString();

            var matchingStandard = new GetStandardsListItem
            {
                LarsCode = expectedLarsCode,
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

            var apiResponse = new ApiResponse<PostVacancyResponse>(responseValue, HttpStatusCode.OK, "");
            mockRecruitApiClient.Setup(x =>
                x.PostWithResponseCode<PostVacancyResponse>(
                    It.IsAny<PostVacancyV2Request>(), true))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyV2RequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body.VacancyReference.ToString());
            mockRecruitApiClient.Verify(client => client.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), true),
                Times.Once);
        }

        [Test, MoqAutoData]
        public Task Then_The_Command_Is_Handled_When_Provider_Is_Null_Throws_Exception(
            PostVacancyResponse responseValue,
            CreateVacancyCommand command,
            AccountLegalEntityItem accountLegalEntityItem,
            ProviderDetailsModel trainingProviderDetails,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
            [Frozen] Mock<ICourseService> courseServiceMock,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            command.PostVacancyV2RequestData.OwnerType = OwnerType.Employer;
            command.IsSandbox = false;
            var expectedLarsCode = 123;
            command.PostVacancyV2RequestData.ProgrammeId = expectedLarsCode.ToString();

            var matchingStandard = new GetStandardsListItem
            {
                LarsCode = expectedLarsCode,
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

            var apiResponse = new ApiResponse<PostVacancyResponse>(responseValue, HttpStatusCode.OK, "");
            mockRecruitApiClient.Setup(x =>
                    x.PostWithResponseCode<PostVacancyResponse>(
                        It.IsAny<PostVacancyV2Request>(), true))
                .ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(command.AccountIdentifier)),
                    command.PostVacancyV2RequestData.AccountLegalEntityPublicHashedId))
                .ReturnsAsync(accountLegalEntityItem);

            //Act
            Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            mockRecruitApiClient.Verify(x =>
                x.PostWithResponseCode<PostVacancyResponse>(
                    It.IsAny<PostVacancyV2Request>(), true), Times.Never);
            return Task.CompletedTask;
        }

        public class CreateVacancyCommandHandlerTests
        {
            [Test, MoqAutoData]
            public async Task Then_Throws_SecurityException_When_AccountLegalEntity_Is_Null(
                CreateVacancyCommand command,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
                CreateVacancyCommandHandler handler)
            {
                // Arrange
                accountLegalEntityPermissionService
                    .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                    .ReturnsAsync((AccountLegalEntityItem)null);

                // Act
                Func<Task> act = () => handler.Handle(command, CancellationToken.None);

                // Assert
                await act.Should().ThrowAsync<SecurityException>();
                mockRecruitApiClient.Verify(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), It.IsAny<bool>()), Times.Never);
            }

            [Test, MoqAutoData]
            public async Task Then_Throws_When_TrainingProvider_Is_Null(
                CreateVacancyCommand command,
                AccountLegalEntityItem accountLegalEntityItem,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<ITrainingProviderService> trainingProviderService,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
                CreateVacancyCommandHandler handler)
            {
                // Arrange
                accountLegalEntityPermissionService
                    .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                    .ReturnsAsync(accountLegalEntityItem);

                trainingProviderService
                    .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                    .ReturnsAsync((ProviderDetailsModel)null);

                // Act
                Func<Task> act = () => handler.Handle(command, CancellationToken.None);

                // Assert
                await act.Should().ThrowAsync<Exception>().WithMessage("Training Provider UKPRN not valid");
                mockRecruitApiClient.Verify(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), It.IsAny<bool>()), Times.Never);
            }

            [Test, MoqAutoData]
            public async Task Then_Throws_When_TrainingProvider_Is_Not_Main_Or_Employer_Profile(
                CreateVacancyCommand command,
                AccountLegalEntityItem accountLegalEntityItem,
                ProviderDetailsModel trainingProviderDetails,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<ITrainingProviderService> trainingProviderService,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
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
                mockRecruitApiClient.Verify(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), It.IsAny<bool>()), Times.Never);
            }

            [Test, MoqAutoData]
            public async Task Then_Throws_When_Course_StartDate_Exceeds_LastDateStarts(
                CreateVacancyCommand command,
                AccountLegalEntityItem accountLegalEntityItem,
                ProviderDetailsModel trainingProviderDetails,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<ITrainingProviderService> trainingProviderService,
                [Frozen] Mock<ICourseService> courseServiceMock,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
                CreateVacancyCommandHandler handler)
            {
                // Arrange
                trainingProviderDetails.ProviderType = ProviderType.Main;
                var expectedLarsCode = 123;
                command.PostVacancyV2RequestData.ProgrammeId = expectedLarsCode.ToString();
                command.PostVacancyV2RequestData.StartDate = DateTime.UtcNow.AddMonths(6);

                var matchingStandard = new GetStandardsListItem
                {
                    LarsCode = expectedLarsCode,
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
                mockRecruitApiClient.Verify(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), It.IsAny<bool>()), Times.Never);
            }

            [Test, MoqAutoData]
            public async Task Then_Sets_Status_To_Review_When_Employer_Approval_Required(
                PostVacancyResponse responseValue,
                CreateVacancyCommand command,
                AccountLegalEntityItem accountLegalEntityItem,
                ProviderDetailsModel trainingProviderDetails,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
                [Frozen] Mock<ICourseService> courseServiceMock,
                [Frozen] Mock<ITrainingProviderService> trainingProviderService,
                CreateVacancyCommandHandler handler)
            {
                // Arrange
                command.AccountIdentifier = new AccountIdentifier("Provider-ABC123-Product");
                command.PostVacancyV2RequestData.OwnerType = OwnerType.Provider;
                var expectedLarsCode = 123;
                command.PostVacancyV2RequestData.ProgrammeId = expectedLarsCode.ToString();

                var matchingStandard = new GetStandardsListItem
                {
                    LarsCode = expectedLarsCode,
                    ApprenticeshipType = ApprenticeshipType.Apprenticeship
                };

                accountLegalEntityPermissionService
                    .Setup(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(), It.IsAny<string>()))
                    .ReturnsAsync(accountLegalEntityItem);

                accountLegalEntityPermissionService
                    .Setup(x => x.HasProviderGotEmployersPermissionAsync(
                        It.IsAny<int>(),
                        It.IsAny<long>(),
                        It.IsAny<List<Operation>>()))
                    .ReturnsAsync(false);

                trainingProviderService
                    .Setup(s => s.GetProviderDetails(It.IsAny<int>()))
                    .ReturnsAsync(trainingProviderDetails);

                courseServiceMock
                    .Setup(s => s.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                    .ReturnsAsync(new GetStandardsListResponse { Standards = [matchingStandard] });

                var apiResponse = new ApiResponse<PostVacancyResponse>(responseValue, HttpStatusCode.OK, "");
                mockRecruitApiClient
                    .Setup(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), true))
                    .ReturnsAsync(apiResponse);

                // Act
                await handler.Handle(command, CancellationToken.None);

                // Assert
                command.PostVacancyV2RequestData.Status.Should().Be(nameof(VacancyStatus.Review));
            }

            [Test, MoqAutoData]
            public async Task Then_Sets_Status_To_Submitted_When_Employer_Approval_Not_Required(
                PostVacancyResponse responseValue,
                CreateVacancyCommand command,
                AccountLegalEntityItem accountLegalEntityItem,
                ProviderDetailsModel trainingProviderDetails,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
                [Frozen] Mock<ICourseService> courseServiceMock,
                [Frozen] Mock<ITrainingProviderService> trainingProviderService,
                CreateVacancyCommandHandler handler)
            {
                // Arrange
                command.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
                command.PostVacancyV2RequestData.OwnerType = OwnerType.Employer;
                var expectedLarsCode = 123;
                command.PostVacancyV2RequestData.ProgrammeId = expectedLarsCode.ToString();

                var matchingStandard = new GetStandardsListItem
                {
                    LarsCode = expectedLarsCode,
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

                var apiResponse = new ApiResponse<PostVacancyResponse>(responseValue, HttpStatusCode.OK, "");
                mockRecruitApiClient
                    .Setup(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), true))
                    .ReturnsAsync(apiResponse);

                // Act
                await handler.Handle(command, CancellationToken.None);

                // Assert
                command.PostVacancyV2RequestData.Status.Should().Be(nameof(VacancyStatus.Submitted));
                command.PostVacancyV2RequestData.SubmittedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            }

            [Test, MoqAutoData]
            public async Task Then_Clears_Qualifications_And_Skills_For_Foundation_Apprenticeship(
                PostVacancyResponse responseValue,
                CreateVacancyCommand command,
                AccountLegalEntityItem accountLegalEntityItem,
                ProviderDetailsModel trainingProviderDetails,
                [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
                [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockRecruitApiClient,
                [Frozen] Mock<ICourseService> courseServiceMock,
                [Frozen] Mock<ITrainingProviderService> trainingProviderService,
                CreateVacancyCommandHandler handler)
            {
                // Arrange
                trainingProviderDetails.ProviderType = ProviderType.Employer;
                var expectedLarsCode = 123;
                command.PostVacancyV2RequestData.ProgrammeId = expectedLarsCode.ToString();

                var matchingStandard = new GetStandardsListItem
                {
                    LarsCode = expectedLarsCode,
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

                var apiResponse = new ApiResponse<PostVacancyResponse>(responseValue, HttpStatusCode.OK, "");
                mockRecruitApiClient
                    .Setup(x => x.PostWithResponseCode<PostVacancyResponse>(It.IsAny<PostVacancyV2Request>(), true))
                    .ReturnsAsync(apiResponse);

                // Act
                await handler.Handle(command, CancellationToken.None);

                // Assert
                command.PostVacancyV2RequestData.Qualifications.Should().BeEmpty();
                command.PostVacancyV2RequestData.Skills.Should().BeEmpty();
                command.PostVacancyV2RequestData.ApprenticeshipType.Should().Be("Foundation");
            }
        }
    }
}