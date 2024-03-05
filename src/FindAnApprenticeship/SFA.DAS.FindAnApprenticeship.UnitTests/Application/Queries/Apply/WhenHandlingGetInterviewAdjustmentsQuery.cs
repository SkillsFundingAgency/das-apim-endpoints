using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetInterviewAdjustmentsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetInterviewAdjustmentsQuery query,
        GetInterviewAdjustmentsApiResponse interviewAdjustmentsApiResponse,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetInterviewAdjustmentsQueryHandler handler)
    {
        var expectedGetInterviewAdjustmentsRequest = new GetInterviewAdjustmentsApiRequest(query.ApplicationId, query.CandidateId);
        candidateApiClient
            .Setup(client => client.Get<GetInterviewAdjustmentsApiResponse>(
                It.Is<GetInterviewAdjustmentsApiRequest>(r => r.GetUrl == expectedGetInterviewAdjustmentsRequest.GetUrl)))
            .ReturnsAsync(interviewAdjustmentsApiResponse);

        var expectedApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);
        applicationApiResponse.InterviewAdjustmentsStatus = Domain.Constants.SectionStatus.InProgress;
        candidateApiClient.Setup(client =>
                client.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(new GetInterviewAdjustmentsQueryResult
        {
            IsSectionCompleted = false,
            InterviewAdjustmentsDescription = interviewAdjustmentsApiResponse.Support,
            ApplicationId = applicationApiResponse.Id
        });
    }
}
