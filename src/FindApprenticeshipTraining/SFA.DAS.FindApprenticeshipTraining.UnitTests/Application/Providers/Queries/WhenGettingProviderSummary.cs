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
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Providers.Queries;
public class WhenGettingProviderSummary
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ProviderSummary_From_RoatpApi(
        int ukprn,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        [Greedy] GetProviderSummaryQueryHandler handler,
        GetProviderSummaryQueryResponse providerSummaryResponse,
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
    public async Task Then_Gets_Provider_Courses(
       int ukprn,
       [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
       [Greedy] GetProviderSummaryQueryHandler handler,
       List<GetProviderAdditionalStandardsItem> coursesResponse,
       CancellationToken cancellationToken
   )
    {
        var query = new GetProviderSummaryQuery(ukprn);

        apiClient.Setup(x =>
                x.Get<List<GetProviderAdditionalStandardsItem>>(
                    It.Is<GetProviderAdditionalStandardsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(coursesResponse);

        var sut = await handler.Handle(query, cancellationToken);

        sut.Courses.Should().BeEquivalentTo(coursesResponse,
            options => options
                .Excluding(c => c.IsApprovedByRegulator)
                .Excluding(c => c.ApprovalBody)
                .Excluding(c => c.LarsCode)
            );

        sut.Courses[0].LarsCode.Should().Be(coursesResponse[0].LarsCode.ToString());
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Provider_Assessment_Summary(
     int ukprn,
     [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> assessorsApiClient,
     [Greedy] GetProviderSummaryQueryHandler handler,
     GetEndpointAssessmentsResponse assessorResponse,
     CancellationToken cancellationToken
 )
    {
        var query = new GetProviderSummaryQuery(ukprn);

        assessorsApiClient.Setup(x =>
                x.Get<GetEndpointAssessmentsResponse>(
                    It.Is<GetEndpointAssessmentsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(assessorResponse);

        var sut = await handler.Handle(query, cancellationToken);

        sut.EndpointAssessments.Should().BeEquivalentTo(assessorResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Provider_Feedback(
        int ukprn,
        [Frozen] Mock<ICachedFeedbackService> cachedFeedbackService,
        [Greedy] GetProviderSummaryQueryHandler handler,
        EmployerFeedbackAnnualDetails feedbackResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        CancellationToken cancellationToken
)
    {
        var query = new GetProviderSummaryQuery(ukprn);

        cachedFeedbackService.Setup(x =>
                x.GetProviderFeedback(ukprn))
            .ReturnsAsync((feedbackResponse, apprenticeFeedbackResponse));

        var sut = await handler.Handle(query, cancellationToken);
        sut.AnnualEmployerFeedbackDetails.Should().BeEquivalentTo(feedbackResponse.AnnualEmployerFeedbackDetails);
        sut.AnnualApprenticeFeedbackDetails.Should().BeEquivalentTo(apprenticeFeedbackResponse.AnnualApprenticeFeedbackDetails);
    }
}
