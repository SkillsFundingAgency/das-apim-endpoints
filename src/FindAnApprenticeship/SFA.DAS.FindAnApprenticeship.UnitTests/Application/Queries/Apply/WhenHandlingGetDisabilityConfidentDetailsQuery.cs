using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetDisabilityConfidentDetailsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(GetDisabilityConfidentDetailsQuery query,
            GetApplicationApiResponse applicationApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetDisabilityConfidentDetailsQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);

            candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);
            

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            bool? expectSectionCompleted = applicationApiResponse.DisabilityConfidenceStatus switch
            {
                Constants.SectionStatus.InProgress => false,
                Constants.SectionStatus.Completed => true,
                _ => null
            };

            result.Should().BeEquivalentTo(new
            {
                applicationApiResponse.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = expectSectionCompleted
            });
        }

        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Return_Null_As_Expected(GetDisabilityConfidentDetailsQuery query,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetDisabilityConfidentDetailsQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);

            candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync((GetApplicationApiResponse)null!);


            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().BeNull();
        }
    }
}
