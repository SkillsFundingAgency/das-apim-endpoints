using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Providers.Queries;
public class WhenGettingProviderSummary
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ProviderSummary_Check_Results_From_RoatpApi(
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
            sut.ProviderAddress.Should().BeEquivalentTo(providerSummaryResponse.Address);
            sut.Contact.MarketingInfo.Should().Be(providerSummaryResponse.MarketingInfo);
            sut.Contact.Email.Should().Be(providerSummaryResponse.Email);
            sut.Contact.PhoneNumber.Should().Be(providerSummaryResponse.Phone);
            sut.Contact.Website.Should().Be(providerSummaryResponse.ContactUrl);
            sut.Qar.Should().BeEquivalentTo(providerSummaryResponse.Qar);
            sut.Reviews.Should().BeEquivalentTo(providerSummaryResponse.Reviews);
        }
    }


    [Test, MoqAutoData]
    public async Task Then_Gets_ProviderSummary_Check_Results_From_Courses(
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
            );

    }

    [Test, MoqAutoData]
    public async Task Then_Gets_ProviderSummary_Check_Results_From_Assessor(
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
    public async Task Then_Gets_ProviderSummary_Check_Results_From_EmployerFeedback(
        int ukprn,
        [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> employerFeedbackApiClient,
        [Greedy] GetProviderSummaryQueryHandler handler,
        EmployerFeedbackAnnualDetails feedbackResponse,
        CancellationToken cancellationToken
)
    {
        var query = new GetProviderSummaryQuery(ukprn);

        employerFeedbackApiClient.Setup(x =>
                x.Get<EmployerFeedbackAnnualDetails>(
                    It.Is<GetEmployerFeedbackDetailsAnnualRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(feedbackResponse);

        var sut = await handler.Handle(query, cancellationToken);
        sut.AnnualEmployerFeedbackDetails.Should().BeEquivalentTo(feedbackResponse.AnnualEmployerFeedbackDetails);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_ProviderSummary_Check_Results_From_ApprenticeFeedback(
       int ukprn,
       [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> apprenticeFeedbackApiClient,
       [Greedy] GetProviderSummaryQueryHandler handler,
       ApprenticeFeedbackAnnualDetails feedbackResponse,
       CancellationToken cancellationToken
)
    {
        var query = new GetProviderSummaryQuery(ukprn);

        apprenticeFeedbackApiClient.Setup(x =>
                x.Get<ApprenticeFeedbackAnnualDetails>(
                    It.Is<GetApprenticeFeedbackDetailsAnnualRequest>(c => c.GetUrl.Contains(ukprn.ToString()))
                )
            )
            .ReturnsAsync(feedbackResponse);

        var sut = await handler.Handle(query, cancellationToken);
        sut.AnnualApprenticeFeedbackDetails.Should().BeEquivalentTo(feedbackResponse.AnnualApprenticeFeedbackDetails);
    }
}
