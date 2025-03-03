using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualification;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.Qualifications;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetDeleteQualificationQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetDeleteQualificationQuery query,
            GetQualificationApiResponse qualificationApiResponse,
            GetApplicationApiResponse applicationApiResponse,
            GetQualificationReferenceTypesApiResponse referenceDataApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetDeleteQualificationQueryHandler handler)
        {
            referenceDataApiResponse.QualificationReferences.First().Id = query.QualificationReference;

            var expectedReferenceDataRequest = new GetQualificationReferenceTypesApiRequest();
            candidateApiClient.Setup(client =>
                    client.Get<GetQualificationReferenceTypesApiResponse>(It.Is<GetQualificationReferenceTypesApiRequest>(r => r.GetUrl == expectedReferenceDataRequest.GetUrl)))
                .ReturnsAsync(referenceDataApiResponse);

            var expectedQualificationApiRequest =
                new GetQualificationApiRequest(query.ApplicationId, query.CandidateId, query.Id);

            candidateApiClient.Setup(x =>
                    x.Get<GetQualificationApiResponse>(
                        It.Is<GetQualificationApiRequest>(r => r.GetUrl == expectedQualificationApiRequest.GetUrl)))
                .ReturnsAsync(qualificationApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            result.QualificationReference.Should().Be(referenceDataApiResponse.QualificationReferences.First().Name);
            result.Qualifications.First().Should().BeEquivalentTo(qualificationApiResponse.Qualification);
        }

        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Null_And_Returned_As_Expected(
            GetDeleteQualificationQuery query,
            GetApplicationApiResponse applicationApiResponse,
            GetQualificationReferenceTypesApiResponse referenceDataApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetDeleteQualificationQueryHandler handler)
        {
            referenceDataApiResponse.QualificationReferences.First().Id = query.QualificationReference;

            var expectedReferenceDataRequest = new GetQualificationReferenceTypesApiRequest();
            candidateApiClient.Setup(client =>
                    client.Get<GetQualificationReferenceTypesApiResponse>(It.Is<GetQualificationReferenceTypesApiRequest>(r => r.GetUrl == expectedReferenceDataRequest.GetUrl)))
                .ReturnsAsync(referenceDataApiResponse);

            var expectedQualificationApiRequest =
                new GetQualificationApiRequest(query.ApplicationId, query.CandidateId, query.Id);

            candidateApiClient.Setup(x =>
                    x.Get<GetQualificationApiResponse>(
                        It.Is<GetQualificationApiRequest>(r => r.GetUrl == expectedQualificationApiRequest.GetUrl)))
                .ReturnsAsync((GetQualificationApiResponse)null!);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            result.Should().BeNull();
        }
    }
}
