using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
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
            GetCandidateApiResponse candidateApiResponse,
            GetAddressApiResponse addressApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationQueryHandler handler)
        {
            var expectedGetCandidateRequest = new GetCandidateApiRequest(query.CandidateId.ToString());
            candidateApiClient
                .Setup(client => client.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(candidateApiResponse);

            var expectedGetAddressRequest = new GetCandidateAddressApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetAddressApiResponse>(
                    It.Is<GetCandidateAddressApiRequest>(r => r.GetUrl == expectedGetAddressRequest.GetUrl)))
                .ReturnsAsync(addressApiResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.CandidateDetails.Address.Should().BeEquivalentTo(addressApiResponse, options => options.Excluding(fil => fil.CandidateId));
            result.CandidateDetails.Should().BeEquivalentTo(candidateApiResponse);
        }
    }
}
