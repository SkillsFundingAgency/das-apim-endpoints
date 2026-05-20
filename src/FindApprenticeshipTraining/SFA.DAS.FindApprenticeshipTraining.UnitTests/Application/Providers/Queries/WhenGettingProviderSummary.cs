using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Providers.Queries;

public class WhenGettingProviderSummary
{
    [Test, MoqAutoData]
    public async Task Handle_ValidDependenciesResponse_ReturnsProviderSummaryFromRoatpApi(
        int ukprn,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> assessorsApiClient,
        [Frozen] Mock<ICachedFeedbackService> cachedFeedbackService,
        [Greedy] GetProviderSummaryQueryHandler handler,
        GetProviderSummaryQueryResponse providerSummaryResponse,
        List<GetProviderAdditionalStandardsItem> coursesResponse,
        GetEndpointAssessmentsResponse assessorResponse,
        EmployerFeedbackAnnualDetails feedbackResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        CancellationToken cancellationToken
    )
    {
        var query = new GetProviderSummaryQuery(ukprn);

        apiClient.Setup(x =>
                x.Get<GetProviderSummaryQueryResponse>(
                    It.Is<GetProviderSummaryRequest>(x => x.Ukprn == ukprn)
                )
            )
            .ReturnsAsync(providerSummaryResponse);

        apiClient.Setup(x =>
                x.Get<List<GetProviderAdditionalStandardsItem>>(
                    It.Is<GetProviderAdditionalStandardsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(coursesResponse);

        assessorsApiClient.Setup(x =>
                x.Get<GetEndpointAssessmentsResponse>(
                    It.Is<GetEndpointAssessmentsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(assessorResponse);

        cachedFeedbackService.Setup(x =>
                x.GetProviderFeedback(ukprn))
            .ReturnsAsync((feedbackResponse, apprenticeFeedbackResponse));

        var sut = await handler.Handle(query, cancellationToken);

        using (new AssertionScope())
        {
            sut.Ukprn.Should().Be(providerSummaryResponse.Ukprn);
            sut.ProviderName.Should().Be(providerSummaryResponse.Name);
            sut.ProviderAddress.Should().BeEquivalentTo(providerSummaryResponse.Address, options => options
                .Excluding(x => x.Latitude)
                .Excluding(x => x.Longitude));
            sut.Contact.MarketingInfo.Should().Be(providerSummaryResponse.MarketingInfo);
            sut.Contact.Email.Should().Be(providerSummaryResponse.Email);
            sut.Contact.PhoneNumber.Should().Be(providerSummaryResponse.Phone);
            sut.Contact.Website.Should().Be(providerSummaryResponse.ContactUrl);
            sut.Qar.Should().BeEquivalentTo(providerSummaryResponse.Qar);
            sut.Reviews.Should().BeEquivalentTo(providerSummaryResponse.Reviews);
        }
    }


    [Test, MoqAutoData]
    public async Task Handle_ValidDependenciesResponse_ReturnsProviderCourses(
       int ukprn,
       [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
       [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> assessorsApiClient,
       [Frozen] Mock<ICachedFeedbackService> cachedFeedbackService,
       [Greedy] GetProviderSummaryQueryHandler handler,
       GetProviderSummaryQueryResponse providerSummaryResponse,
       List<GetProviderAdditionalStandardsItem> coursesResponse,
       GetEndpointAssessmentsResponse assessorResponse,
       EmployerFeedbackAnnualDetails feedbackResponse,
       ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
       CancellationToken cancellationToken
   )
    {
        var query = new GetProviderSummaryQuery(ukprn);

        apiClient.Setup(x =>
                x.Get<GetProviderSummaryQueryResponse>(
                    It.Is<GetProviderSummaryRequest>(x => x.Ukprn == ukprn)
                )
            )
            .ReturnsAsync(providerSummaryResponse);

        apiClient.Setup(x =>
                x.Get<List<GetProviderAdditionalStandardsItem>>(
                    It.Is<GetProviderAdditionalStandardsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(coursesResponse);

        assessorsApiClient.Setup(x =>
                x.Get<GetEndpointAssessmentsResponse>(
                    It.Is<GetEndpointAssessmentsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(assessorResponse);

        cachedFeedbackService.Setup(x =>
                x.GetProviderFeedback(ukprn))
            .ReturnsAsync((feedbackResponse, apprenticeFeedbackResponse));

        var sut = await handler.Handle(query, cancellationToken);

        sut.Courses.Should().BeEquivalentTo(coursesResponse,
            options => options
                .Excluding(c => c.IsApprovedByRegulator)
                .Excluding(c => c.ApprovalBody)
                .Excluding(c => c.LarsCode)
            );

        sut.Courses[0].LarsCode.Should().Be(coursesResponse[0].LarsCode);
    }

    [Test, MoqAutoData]
    public async Task Handle_ValidDependenciesResponse_ReturnsProviderAssessmentSummary(
     int ukprn,
     [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
     [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> assessorsApiClient,
     [Frozen] Mock<ICachedFeedbackService> cachedFeedbackService,
     [Greedy] GetProviderSummaryQueryHandler handler,
     GetProviderSummaryQueryResponse providerSummaryResponse,
     List<GetProviderAdditionalStandardsItem> coursesResponse,
     GetEndpointAssessmentsResponse assessorResponse,
     EmployerFeedbackAnnualDetails feedbackResponse,
     ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
     CancellationToken cancellationToken
 )
    {
        var query = new GetProviderSummaryQuery(ukprn);

        apiClient.Setup(x =>
                x.Get<GetProviderSummaryQueryResponse>(
                    It.Is<GetProviderSummaryRequest>(x => x.Ukprn == ukprn)
                )
            )
            .ReturnsAsync(providerSummaryResponse);

        apiClient.Setup(x =>
                x.Get<List<GetProviderAdditionalStandardsItem>>(
                    It.Is<GetProviderAdditionalStandardsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(coursesResponse);

        assessorsApiClient.Setup(x =>
                x.Get<GetEndpointAssessmentsResponse>(
                    It.Is<GetEndpointAssessmentsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(assessorResponse);

        cachedFeedbackService.Setup(x =>
                x.GetProviderFeedback(ukprn))
            .ReturnsAsync((feedbackResponse, apprenticeFeedbackResponse));

        var sut = await handler.Handle(query, cancellationToken);

        sut.EndpointAssessments.Should().BeEquivalentTo(assessorResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_ValidDependenciesResponse_ReturnsProviderFeedback(
        int ukprn,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> assessorsApiClient,
        [Frozen] Mock<ICachedFeedbackService> cachedFeedbackService,
        [Greedy] GetProviderSummaryQueryHandler handler,
        GetProviderSummaryQueryResponse providerSummaryResponse,
        List<GetProviderAdditionalStandardsItem> coursesResponse,
        GetEndpointAssessmentsResponse assessorResponse,
        EmployerFeedbackAnnualDetails feedbackResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        CancellationToken cancellationToken
)
    {
        var query = new GetProviderSummaryQuery(ukprn);

        apiClient.Setup(x =>
                x.Get<GetProviderSummaryQueryResponse>(
                    It.Is<GetProviderSummaryRequest>(x => x.Ukprn == ukprn)
                )
            )
            .ReturnsAsync(providerSummaryResponse);

        apiClient.Setup(x =>
                x.Get<List<GetProviderAdditionalStandardsItem>>(
                    It.Is<GetProviderAdditionalStandardsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(coursesResponse);

        assessorsApiClient.Setup(x =>
                x.Get<GetEndpointAssessmentsResponse>(
                    It.Is<GetEndpointAssessmentsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(assessorResponse);

        cachedFeedbackService.Setup(x =>
                x.GetProviderFeedback(ukprn))
            .ReturnsAsync((feedbackResponse, apprenticeFeedbackResponse));

        var sut = await handler.Handle(query, cancellationToken);
        sut.AnnualEmployerFeedbackDetails.Should().BeEquivalentTo(feedbackResponse.AnnualEmployerFeedbackDetails);
        sut.AnnualApprenticeFeedbackDetails.Should().BeEquivalentTo(apprenticeFeedbackResponse.AnnualApprenticeFeedbackDetails);
    }
}
