using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetQualificationsQuery query,
            GetQualificationsApiResponse qualificationsApiResponse,
            GetApplicationApiResponse applicationApiResponse,
            GetQualificationReferenceTypesApiResponse referenceDataApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetQualificationsQueryHandler handler)
        {
            applicationApiResponse.QualificationsStatus = Constants.SectionStatus.Incomplete;

            var expectedApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
            applicationApiResponse.JobsStatus = Constants.SectionStatus.Incomplete;
            candidateApiClient.Setup(client =>
                client.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var expectedReferenceDataRequest = new GetQualificationReferenceTypesApiRequest();
            candidateApiClient.Setup(client =>
                    client.Get<GetQualificationReferenceTypesApiResponse>(It.Is<GetQualificationReferenceTypesApiRequest>(r => r.GetUrl == expectedReferenceDataRequest.GetUrl)))
                .ReturnsAsync(referenceDataApiResponse);

            var expectedQualificationsApiRequest =
                new GetQualificationsApiRequest(query.ApplicationId, query.CandidateId);
            candidateApiClient.Setup(x =>
                    x.Get<GetQualificationsApiResponse>(
                        It.Is<GetQualificationsApiRequest>(r => r.GetUrl == expectedQualificationsApiRequest.GetUrl)))
                .ReturnsAsync(qualificationsApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            result.IsSectionCompleted.Should().BeFalse();
            result.QualificationTypes.Should().BeEquivalentTo(referenceDataApiResponse.QualificationReferences, options=>options.ExcludingMissingMembers());
            result.Qualifications.Should().BeEquivalentTo(qualificationsApiResponse.Qualifications, options=>options.ExcludingMissingMembers());
            
        }
    }
}
