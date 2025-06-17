using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

[TestFixture]
public class AddPriorLearningDataCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_When_All_Data_Valid_Returns_Success(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        [Frozen] Mock<ILogger<AddPriorLearningDataCommandHandler>> logger,
        GetDraftApprenticeshipResponse apprenticeship,
        GetStandardsListItem standardResponse,
        GetRecognitionOfPriorLearningResponse priorLearningResponse,
        GetPriorLearningSummaryResponse priorLearningSummary,
        AddPriorLearningDataCommand request,
        AddPriorLearningDataResponse expectedResponse,
        AddPriorLearningDataCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = "Apprenticeship";
        apprenticeship.CourseCode = "123";
        apprenticeship.HasStandardOptions = true;
        priorLearningSummary.RplPriceReductionError = false;
        priorLearningResponse.OffTheJobTrainingMinimumHours = 100;

        commitmentsApiClient
            .Setup(x => x.Get<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(p => 
                p.CohortId == request.CohortId && p.DraftApprenticeshipId == request.DraftApprenticeshipId)))
            .ReturnsAsync(apprenticeship)
            .Verifiable();

        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(apprenticeship.CourseCode))))
            .ReturnsAsync(standardResponse)
            .Verifiable();

        courseTypesApiClient
            .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(
                It.Is<GetRecognitionOfPriorLearningRequest>(x => x.GetUrl.Contains(standardResponse.ApprenticeshipType))))
            .ReturnsAsync(priorLearningResponse)
            .Verifiable();

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<AddPriorLearningDataResponse>(
                It.Is<PostAddPriorLearningDataRequest>(r =>
                    r.CohortId == request.CohortId &&
                    r.DraftApprenticeshipId == request.DraftApprenticeshipId &&
                    ((AddPriorLearningDataRequest)r.Data).DurationReducedBy == request.DurationReducedBy &&
                    ((AddPriorLearningDataRequest)r.Data).DurationReducedByHours == request.DurationReducedByHours &&
                    ((AddPriorLearningDataRequest)r.Data).IsDurationReducedByRpl == request.IsDurationReducedByRpl &&
                    ((AddPriorLearningDataRequest)r.Data).PriceReducedBy == request.PriceReducedBy &&
                    ((AddPriorLearningDataRequest)r.Data).TrainingTotalHours == request.TrainingTotalHours &&
                    ((AddPriorLearningDataRequest)r.Data).MinimumOffTheJobTrainingHoursRequired == priorLearningResponse.OffTheJobTrainingMinimumHours
                ), false))
            .ReturnsAsync(new ApiResponse<AddPriorLearningDataResponse>(expectedResponse, HttpStatusCode.OK, string.Empty))
            .Verifiable();

        commitmentsApiClient
            .Setup(x => x.Get<GetPriorLearningSummaryResponse>(It.Is<GetPriorLearningSummaryRequest>(p => 
                p.CohortId == request.CohortId && p.DraftApprenticeshipId == request.DraftApprenticeshipId)))
            .ReturnsAsync(priorLearningSummary)
            .Verifiable();

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.HasStandardOptions.Should().Be(apprenticeship.HasStandardOptions);
        response.RplPriceReductionError.Should().Be(priorLearningSummary.RplPriceReductionError);
        commitmentsApiClient.Verify();
        coursesApiClient.Verify();
        courseTypesApiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task Handle_When_Standard_Not_Found_Throws_Exception(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        [Frozen] Mock<ILogger<AddPriorLearningDataCommandHandler>> logger,
        GetDraftApprenticeshipResponse apprenticeship,
        AddPriorLearningDataCommand request,
        AddPriorLearningDataCommandHandler handler)
    {
        // Arrange
        apprenticeship.CourseCode = "123";

        commitmentsApiClient
            .Setup(x => x.Get<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(p => 
                p.CohortId == request.CohortId && p.DraftApprenticeshipId == request.DraftApprenticeshipId)))
            .ReturnsAsync(apprenticeship)
            .Verifiable();

        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(apprenticeship.CourseCode))))
            .ReturnsAsync((GetStandardsListItem)null)
            .Verifiable();

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Standard not found for course ID {apprenticeship.CourseCode}");
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Standard not found for course ID {apprenticeship.CourseCode}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_When_RPL_Rules_Not_Found_Throws_Exception(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        [Frozen] Mock<ILogger<AddPriorLearningDataCommandHandler>> logger,
        GetDraftApprenticeshipResponse apprenticeship,
        GetStandardsListItem standardResponse,
        AddPriorLearningDataCommand request,
        AddPriorLearningDataCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = "Apprenticeship";
        apprenticeship.CourseCode = "123";

        commitmentsApiClient
            .Setup(x => x.Get<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(p => 
                p.CohortId == request.CohortId && p.DraftApprenticeshipId == request.DraftApprenticeshipId)))
            .ReturnsAsync(apprenticeship)
            .Verifiable();

        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(apprenticeship.CourseCode))))
            .ReturnsAsync(standardResponse)
            .Verifiable();

        courseTypesApiClient
            .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(
                It.Is<GetRecognitionOfPriorLearningRequest>(x => x.GetUrl.Contains(standardResponse.ApprenticeshipType))))
            .ReturnsAsync((GetRecognitionOfPriorLearningResponse)null)
            .Verifiable();

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"RPL rules not found for apprenticeship type {standardResponse.ApprenticeshipType}");
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"RPL rules not found for apprenticeship type {standardResponse.ApprenticeshipType}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }
}