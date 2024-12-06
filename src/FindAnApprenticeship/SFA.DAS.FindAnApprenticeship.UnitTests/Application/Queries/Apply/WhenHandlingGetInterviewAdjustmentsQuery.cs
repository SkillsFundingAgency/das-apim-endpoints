using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetInterviewAdjustmentsQuery
{
    [Test]
    [MoqInlineAutoData(false, Constants.SectionStatus.Incomplete)]
    [MoqInlineAutoData(true, Constants.SectionStatus.Completed)]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        bool isSectionCompleted,
        string sectionStatus,
        GetInterviewAdjustmentsQuery query,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetInterviewAdjustmentsQueryHandler handler)
    {
        applicationApiResponse.InterviewAdjustmentsStatus = sectionStatus;
        
        var expectedApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        candidateApiClient.Setup(client =>
                client.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(new GetInterviewAdjustmentsQueryResult
        {
            IsSectionCompleted = isSectionCompleted,
            InterviewAdjustmentsDescription = applicationApiResponse.Support,
            ApplicationId = applicationApiResponse.Id
        });
    }
}
