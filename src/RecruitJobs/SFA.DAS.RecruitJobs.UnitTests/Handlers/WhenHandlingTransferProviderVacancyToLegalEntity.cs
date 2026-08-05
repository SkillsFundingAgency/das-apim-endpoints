using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.GraphQL;
using SFA.DAS.RecruitJobs.Handlers;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using ClosureReason = SFA.DAS.Recruit.Contracts.ApiResponses.ClosureReason;
using OwnerType = SFA.DAS.Recruit.Contracts.ApiResponses.OwnerType;
using VacancyStatus = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyStatus;
using TransferInfo = SFA.DAS.Recruit.Contracts.ApiResponses.TransferInfo;
using Vacancy = SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy;
using VacancyReview = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyReview;

namespace SFA.DAS.RecruitJobs.UnitTests.Handlers;

public class WhenHandlingTransferProviderVacancyToLegalEntity
{
    public abstract record MockVacancyDetails(
        Guid Id,
        long? VacancyReference,
        string? Title,
        GraphQL.VacancyStatus Status,
        string? TrainingProvider_Name,
        string? LegalEntityName,
        int? Ukprn) : IGetProviderTransferableVacancyDetails_Vacancies;

    [Test, MoqAutoData]
    public async Task Response_Errors_From_The_Graphql_Api_Throw_An_Exception(
        Guid vacancyId,
        IReadOnlyList<IClientError> errors,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(null, null, null!, errors);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        var action = async () => await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<GraphQLClientException>();
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Vacancies_Are_Available_To_Be_Transferred_Nothing_Happens(
        Guid vacancyId,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        data
            .Setup(x => x.Vacancies)
            .Returns([]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        recruitApiClient.VerifyNoOtherCalls();
    }

    [Test]
    [RecursiveMoqInlineAutoData(VacancyStatus.Draft)]
    [RecursiveMoqInlineAutoData(VacancyStatus.Referred)]
    public async Task Then_The_Vacancy_Is_Transferred(
        GraphQL.VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = status };
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name!,
            LegalEntityName = vacancyDetails.LegalEntityName!,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
    }

    [Test]
    [RecursiveMoqInlineAutoData(VacancyStatus.Rejected)]
    [RecursiveMoqInlineAutoData(VacancyStatus.Review)]
    public async Task Then_The_Vacancy_Is_Transferred_And_Made_Draft_Again(
        GraphQL.VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = status };
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name!,
            LegalEntityName = vacancyDetails.LegalEntityName!,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/status", null, VacancyStatus.Draft));
    }

    [Test]
    [RecursiveMoqInlineAutoData(VacancyStatus.Live)]
    public async Task Then_The_Vacancy_Is_Transferred_And_Closed_And_Applications_Moved_Over(
        GraphQL.VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = status };
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name!,
            LegalEntityName = vacancyDetails.LegalEntityName!,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        var applicationReviews = new List<GetApplicationReviewResponse>
        {
            new() { ApplicationId = Guid.NewGuid(), Status = ApplicationReviewStatus.EmployerInterviewing },
            new() { ApplicationId = Guid.NewGuid(), Status = ApplicationReviewStatus.Shared },
            new() { ApplicationId = Guid.NewGuid(), Status = ApplicationReviewStatus.EmployerUnsuccessful },
        };
        data.Setup(x => x.Vacancies).Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.Get<List<GetApplicationReviewResponse>>(
                It.Is<GetVacanciesByidByVacancyIdApplicationreviewsApiRequest>(c =>
                    c.VacancyId == vacancyId
                    && c.Statuses!.Contains(ApplicationReviewStatus.Shared)
                    && c.Statuses!.Contains(ApplicationReviewStatus.EmployerInterviewing)
                    && c.Statuses!.Contains(ApplicationReviewStatus.EmployerUnsuccessful))))
            .ReturnsAsync(applicationReviews);
        PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));
        var capturedApplicationReviewPatches = new List<PatchApplicationreviewsByApplicationIdApiRequest?>();
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationreviewsByApplicationIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<ApplicationReview>>>(x => capturedApplicationReviewPatches.Add(x as PatchApplicationreviewsByApplicationIdApiRequest))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/status", null, VacancyStatus.Closed));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/closureReason", null, ClosureReason.TransferredByEmployer));
        var closedDate = capturedPatchRequest.Data.Operations.First(x => x.path == "/closedDate").value as DateTime?;
        closedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        capturedApplicationReviewPatches.Should().HaveCount(applicationReviews.Count);
        foreach (var patch in capturedApplicationReviewPatches)
        {
            patch!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<ApplicationReview>("replace", "/dateSharedWithEmployer", null, null));
            patch.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<ApplicationReview>("replace", "/hasEverBeenEmployerInterviewing", null, false));
        }
    }

    [Test]
    [RecursiveMoqInlineAutoData(ApplicationReviewStatus.EmployerInterviewing)]
    [RecursiveMoqInlineAutoData(ApplicationReviewStatus.Shared)]
    public async Task Then_EmployerInterviewing_Or_Shared_Application_Reviews_Are_Reset_To_New(
        ApplicationReviewStatus reviewStatus,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = GraphQL.VacancyStatus.Live };
        var applicationReview = new GetApplicationReviewResponse { ApplicationId = Guid.NewGuid(), Status = reviewStatus };
        data.Setup(x => x.Vacancies).Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));
        recruitApiClient
            .Setup(x => x.Get<List<GetApplicationReviewResponse>>(It.IsAny<GetVacanciesByidByVacancyIdApplicationreviewsApiRequest>()))
            .ReturnsAsync([applicationReview]);
        PatchApplicationreviewsByApplicationIdApiRequest? capturedPatch = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationreviewsByApplicationIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<ApplicationReview>>>(x => capturedPatch = x as PatchApplicationreviewsByApplicationIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        capturedPatch!.Data.Operations.Should().ContainEquivalentOf(
            new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<ApplicationReview>("replace", "/status", null, ApplicationReviewStatus.New));
    }

    [Test]
    [RecursiveMoqInlineAutoData(ApplicationReviewStatus.EmployerUnsuccessful)]
    public async Task Then_EmployerUnsuccessful_Application_Reviews_Are_Made_Unsuccessful(
        ApplicationReviewStatus reviewStatus,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = GraphQL.VacancyStatus.Live };
        var applicationReview = new GetApplicationReviewResponse { ApplicationId = Guid.NewGuid(), Status = reviewStatus };
        data.Setup(x => x.Vacancies).Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));
        recruitApiClient
            .Setup(x => x.Get<List<GetApplicationReviewResponse>>(It.IsAny<GetVacanciesByidByVacancyIdApplicationreviewsApiRequest>()))
            .ReturnsAsync([applicationReview]);
        PatchApplicationreviewsByApplicationIdApiRequest? capturedPatch = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationreviewsByApplicationIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<ApplicationReview>>>(x => capturedPatch = x as PatchApplicationreviewsByApplicationIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        capturedPatch!.Data.Operations.Should().ContainEquivalentOf(
            new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<ApplicationReview>("replace", "/status", null, ApplicationReviewStatus.Unsuccessful));
    }

    [Test]
    [RecursiveMoqInlineAutoData(GraphQL.VacancyStatus.Approved)]
    public async Task Then_The_Vacancy_Is_Transferred_And_Closed_And_Unapproved(
        GraphQL.VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = status };
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name!,
            LegalEntityName = vacancyDetails.LegalEntityName!,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/approvedDate", null, null));
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/status", null, VacancyStatus.Closed));
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/closureReason", null, ClosureReason.TransferredByEmployer));
        var closedDate = capturedPatchRequest.Data.Operations.First(x => x.path == "/closedDate").value as DateTime?;
        closedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Test]
    [RecursiveMoqInlineAutoData(ReviewStatus.Closed)]
    [RecursiveMoqInlineAutoData(ReviewStatus.New)]
    [RecursiveMoqInlineAutoData(ReviewStatus.PendingReview)]
    public async Task Then_The_Submitted_Vacancy_Is_Transferred_And_Made_Draft_As_Long_As_It_Is_Not_Under_Review(
        ReviewStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        List<VacancyReview> vacancyReviews,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = GraphQL.VacancyStatus.Submitted };
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name!,
            LegalEntityName = vacancyDetails.LegalEntityName!,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        vacancyReviews.Add(new VacancyReview
        {
            CreatedDate = DateTime.MaxValue,
            Status = status
        });
        recruitApiClient
            .Setup(x => x.Get<List<VacancyReview>>(It.IsAny<GetVacanciesByVacancyReferenceReviewsApiRequest>()))
            .ReturnsAsync(vacancyReviews);

        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyreviewsByIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/status", null, VacancyStatus.Draft));
    }

    [Test]
    [RecursiveMoqInlineAutoData(ReviewStatus.New)]
    [RecursiveMoqInlineAutoData(ReviewStatus.PendingReview)]
    public async Task Then_New_Or_Pending_Vacancy_Reviews_Are_Closed(
        ReviewStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        List<VacancyReview> vacancyReviews,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = GraphQL.VacancyStatus.Submitted };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        vacancyReviews.Add(new VacancyReview
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.MaxValue,
            Status = status
        });
        recruitApiClient
            .Setup(x => x.Get<List<VacancyReview>>(It.IsAny<GetVacanciesByVacancyReferenceReviewsApiRequest>()))
            .ReturnsAsync(vacancyReviews);

        PatchVacancyreviewsByIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyreviewsByIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<VacancyReview>>>(x => capturedPatchRequest = x as PatchVacancyreviewsByIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<VacancyReview>("replace", "/manualOutcome", null, nameof(Domain.ManualQaOutcome.Transferred)));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<VacancyReview>("replace", "/status", null, ReviewStatus.Closed));
        var closedDate = capturedPatchRequest.Data.Operations.First(x => x.path == "/closedDate").value as DateTime?;
        closedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Test]
    [RecursiveMoqInlineAutoData(ReviewStatus.Closed)]
    [RecursiveMoqInlineAutoData(ReviewStatus.UnderReview)]
    public async Task Then_Closed_Or_Under_Review_Are_Not_Patched(
        ReviewStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        List<VacancyReview> vacancyReviews,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = GraphQL.VacancyStatus.Submitted };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        vacancyReviews.Add(new VacancyReview
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.MaxValue,
            Status = status
        });
        recruitApiClient
            .Setup(x => x.Get<List<VacancyReview>>(It.IsAny<GetVacanciesByVacancyReferenceReviewsApiRequest>()))
            .ReturnsAsync(vacancyReviews);

        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyreviewsByIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyreviewsByIdApiRequest>()), Times.Never);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_A_Closed_Vacancy_Is_Transferred_And_Applications_Are_Moved_Over(
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = GraphQL.VacancyStatus.Closed };
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name!,
            LegalEntityName = vacancyDetails.LegalEntityName!,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        var applicationReviews = new List<GetApplicationReviewResponse>
        {
            new() { ApplicationId = Guid.NewGuid(), Status = ApplicationReviewStatus.EmployerInterviewing },
            new() { ApplicationId = Guid.NewGuid(), Status = ApplicationReviewStatus.Shared },
            new() { ApplicationId = Guid.NewGuid(), Status = ApplicationReviewStatus.EmployerUnsuccessful },
        };
        data.Setup(x => x.Vacancies).Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.Get<List<GetApplicationReviewResponse>>(It.IsAny<GetVacanciesByidByVacancyIdApplicationreviewsApiRequest>()))
            .ReturnsAsync(applicationReviews);
        PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));
        var capturedApplicationReviewPatches = new List<PatchApplicationreviewsByApplicationIdApiRequest?>();
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationreviewsByApplicationIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<ApplicationReview>>>(x => capturedApplicationReviewPatches.Add(x as PatchApplicationreviewsByApplicationIdApiRequest))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().NotContain(x => x.path == "/status");
        capturedPatchRequest.Data.Operations.Should().NotContain(x => x.path == "/closedDate");
        capturedPatchRequest.Data.Operations.Should().NotContain(x => x.path == "/closureReason");
        capturedApplicationReviewPatches.Should().HaveCount(applicationReviews.Count);
        foreach (var patch in capturedApplicationReviewPatches)
        {
            patch!.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<ApplicationReview>("replace", "/dateSharedWithEmployer", null, null));
            patch.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<ApplicationReview>("replace", "/hasEverBeenEmployerInterviewing", null, false));
        }
    }

    private static void CommonAssertions(PatchVacanciesByVacancyIdApiRequest? capturedPatchRequest, TransferInfo expectedTransferInfo)
    {
        // these are common changes irrespective of the vacancy status
        capturedPatchRequest.Should().NotBeNull();
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/ownerType", null, OwnerType.Employer));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/contact", null, null));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/submittedByUserId", null, null));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.Operation<Vacancy>("replace", "/reviewRequestedByUserId", null, null));
        var transferInfo = capturedPatchRequest.Data.Operations.First(x => x.path == "/transferInfo").value as TransferInfo;
        transferInfo.Should().BeEquivalentTo(expectedTransferInfo, opt => opt.Excluding(x => x.TransferredDate));
        transferInfo.TransferredDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
