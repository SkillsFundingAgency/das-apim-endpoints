using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetApplicationQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetApplicationQuery query,
            GetApplicationApiResponse applicationApiResponse,
            GetQualificationReferenceTypesApiResponse qualificationReferenceTypesApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, true);
            applicationApiResponse.ApplicationAllSectionStatus = "completed";
            candidateApiClient
                .Setup(client => client.Get<GetApplicationApiResponse>(
                    It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var expectedGetQualificationTypesApiRequest = new GetQualificationReferenceTypesApiRequest();
            candidateApiClient
                .Setup(client => client.Get<GetQualificationReferenceTypesApiResponse>(
                    It.Is<GetQualificationReferenceTypesApiRequest>(r => r.GetUrl == expectedGetQualificationTypesApiRequest.GetUrl)))
                .ReturnsAsync(qualificationReferenceTypesApiResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.CandidateDetails.Address.Should().BeEquivalentTo(applicationApiResponse.Candidate.Address, options => options.Excluding(fil => fil.CandidateId));
            result.CandidateDetails.Should().BeEquivalentTo(applicationApiResponse.Candidate, options=> options
                    .Excluding(p=>p.MiddleNames)
                    .Excluding(p=>p.DateOfBirth)
                    .Excluding(p=>p.Status)
                    .Excluding(p=>p.Address)
                );
            result.IsApplicationComplete.Should().BeTrue();
        }
    }
}
