using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.RecruitJobs.Handlers;
using SFA.DAS.RecruitJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.UnitTests.Handlers;

public class WhenHandlingTransferProviderVacancyToLegalEntity
{
    public record MockVacancyDetails(
        Guid Id,
        long? VacancyReference,
        string? Title,
        VacancyStatus Status,
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
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
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
    [MoqInlineAutoData(VacancyStatus.Draft)]
    [MoqInlineAutoData(VacancyStatus.Closed)]
    [MoqInlineAutoData(VacancyStatus.Referred)]
    public async Task Then_The_Vacancy_Is_Transferred(
        VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name,
            LegalEntityName = vacancyDetails.LegalEntityName,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        
        vacancyDetails = vacancyDetails with
        {
            Status = status
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null); 
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacancyRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacancyRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
    }
    
    [Test]
    [MoqInlineAutoData(VacancyStatus.Rejected)]
    [MoqInlineAutoData(VacancyStatus.Review)]
    public async Task Then_The_Vacancy_Is_Transferred_And_Made_Draft_Again(
        VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name,
            LegalEntityName = vacancyDetails.LegalEntityName,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        
        vacancyDetails = vacancyDetails with
        {
            Status = status
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null); 
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacancyRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacancyRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/Status", null, VacancyStatus.Draft));
    }
    
    [Test]
    [MoqInlineAutoData(VacancyStatus.Live)]
    public async Task Then_The_Vacancy_Is_Transferred_And_Closed(
        VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name,
            LegalEntityName = vacancyDetails.LegalEntityName,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        
        vacancyDetails = vacancyDetails with
        {
            Status = status
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null); 
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacancyRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacancyRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/Status", null, VacancyStatus.Closed));
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/ClosureReason", null, ClosureReason.TransferredByEmployer));
        var closedDate = capturedPatchRequest.Data.Operations.First(x => x.path == "/ClosedDate").value as DateTime?;
        closedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    [MoqInlineAutoData(VacancyStatus.Approved)]
    public async Task Then_The_Vacancy_Is_Transferred_And_Closed_And_Unapproved(
        VacancyStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name,
            LegalEntityName = vacancyDetails.LegalEntityName,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        
        vacancyDetails = vacancyDetails with
        {
            Status = status
        };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null); 
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacancyRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacancyRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/ApprovedDate", null, null));
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/Status", null, VacancyStatus.Closed));
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/ClosureReason", null, ClosureReason.TransferredByEmployer));
        var closedDate = capturedPatchRequest.Data.Operations.First(x => x.path == "/ClosedDate").value as DateTime?;
        closedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    [MoqInlineAutoData(ReviewStatus.Closed)]
    [MoqInlineAutoData(ReviewStatus.New)]
    [MoqInlineAutoData(ReviewStatus.PendingReview)]
    public async Task Then_The_Submitted_Vacancy_Is_Transferred_And_Made_Draft_As_Long_As_It_Is_Not_Under_Review(
        ReviewStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        List<VacancyReview> vacancyReviews,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        var expectedTransferInfo = new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name,
            LegalEntityName = vacancyDetails.LegalEntityName,
            Reason = TransferReason.EmployerRevokedPermission,
        };
        vacancyDetails = vacancyDetails with { Status = VacancyStatus.Submitted };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        var response = new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null); 
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        PatchVacancyRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedPatchRequest = x as PatchVacancyRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        vacancyReviews.Add(new VacancyReview
        {
            CreatedDate = DateTime.MaxValue,
            Status = status
        });
        recruitApiClient
            .Setup(x => x.GetAll<VacancyReview>(It.IsAny<GetVacancyReviewsByVacancyReferenceRequest>()))
            .ReturnsAsync(vacancyReviews);

        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyReviewRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));
        
        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        CommonAssertions(capturedPatchRequest, expectedTransferInfo);
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/Status", null, VacancyStatus.Draft));
    }
    
    [Test]
    [MoqInlineAutoData(ReviewStatus.New)]
    [MoqInlineAutoData(ReviewStatus.PendingReview)]
    public async Task Then_New_Or_Pending_Vacancy_Reviews_Are_Closed(
        ReviewStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        List<VacancyReview> vacancyReviews,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = VacancyStatus.Submitted };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        vacancyReviews.Add(new VacancyReview
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.MaxValue,
            Status = status
        });
        recruitApiClient
            .Setup(x => x.GetAll<VacancyReview>(It.IsAny<GetVacancyReviewsByVacancyReferenceRequest>()))
            .ReturnsAsync(vacancyReviews);

        PatchVacancyReviewRequest? capturedPatchRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyReviewRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<VacancyReview>>>(x => capturedPatchRequest = x as PatchVacancyReviewRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));
        
        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        capturedPatchRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<VacancyReview>("replace", "/ManualOutcome", null, nameof(ManualQaOutcome.Transferred)));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<VacancyReview>("replace", "/Status", null, ReviewStatus.Closed));
        var closedDate = capturedPatchRequest.Data.Operations.First(x => x.path == "/ClosedDate").value as DateTime?;
        closedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    [MoqInlineAutoData(ReviewStatus.Closed)]
    [MoqInlineAutoData(ReviewStatus.UnderReview)]
    public async Task Then_Closed_Or_Under_Review_Are_Not_Patched(
        ReviewStatus status,
        Guid vacancyId,
        MockVacancyDetails vacancyDetails,
        Mock<IGetProviderTransferableVacancyDetailsResult> data,
        List<VacancyReview> vacancyReviews,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>>  recruitApiClient,
        [Greedy] TransferProviderVacancyToLegalEntityHandler sut)
    {
        // arrange
        vacancyDetails = vacancyDetails with { Status = VacancyStatus.Submitted };
        data
            .Setup(x => x.Vacancies)
            .Returns([vacancyDetails]);
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancyDetails.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<IGetProviderTransferableVacancyDetailsResult>(data.Object, null, null!, null));
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));

        vacancyReviews.Add(new VacancyReview
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.MaxValue,
            Status = status
        });
        recruitApiClient
            .Setup(x => x.GetAll<VacancyReview>(It.IsAny<GetVacancyReviewsByVacancyReferenceRequest>()))
            .ReturnsAsync(vacancyReviews);

        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyReviewRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null));
        
        // act
        await sut.HandleAsync(vacancyId, TransferReason.EmployerRevokedPermission, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchVacancyReviewRequest>()), Times.Never);
    }

    private static void CommonAssertions(PatchVacancyRequest? capturedPatchRequest, TransferInfo expectedTransferInfo)
    {
        // these are common changes irrespective of the vacancy status
        capturedPatchRequest.Should().NotBeNull();
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/OwnerType", null, OwnerType.Employer));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/Contact", null, null));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/SubmittedByUserId", null, null));
        capturedPatchRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/ReviewRequestedByUserId", null, null));
        var transferInfo = capturedPatchRequest.Data.Operations.First(x => x.path == "/TransferInfo").value as TransferInfo;
        transferInfo.Should().BeEquivalentTo(expectedTransferInfo, opt => opt.Excluding(x => x.TransferredDate));
        transferInfo.TransferredDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}