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
            GetCandidateApiResponse candidateApiResponse,
            GetAddressApiResponse addressApiResponse,
            GetAdditionalQuestionApiResponse additionalQuestion1ApiResponse,
            GetAdditionalQuestionApiResponse additionalQuestion2ApiResponse,
            GetApprenticeshipVacancyItemResponse apprenticeshipVacancyItemApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            GetApplicationQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, true);
            applicationApiResponse.ApplicationAllSectionStatus = "completed";
            candidateApiClient
                .Setup(client => client.Get<GetApplicationApiResponse>(
                    It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var expectedGetVacancyRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference);
            findApprenticeshipApiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                    It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
                .ReturnsAsync(apprenticeshipVacancyItemApiResponse);

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

            var expectedGetAdditionalQuestion1ApiRequest = new GetAdditionalQuestionApiRequest(query.ApplicationId, query.CandidateId, applicationApiResponse.AdditionalQuestions[0].Id);
            candidateApiClient
                .Setup(client => client.Get<GetAdditionalQuestionApiResponse>(
                    It.Is<GetAdditionalQuestionApiRequest>(r => r.GetUrl == expectedGetAdditionalQuestion1ApiRequest.GetUrl)))
                .ReturnsAsync(additionalQuestion1ApiResponse);

            var expectedGetAdditionalQuestion2ApiRequest = new GetAdditionalQuestionApiRequest(query.ApplicationId, query.CandidateId, applicationApiResponse.AdditionalQuestions[1].Id);
            candidateApiClient
                .Setup(client => client.Get<GetAdditionalQuestionApiResponse>(
                    It.Is<GetAdditionalQuestionApiRequest>(r => r.GetUrl == expectedGetAdditionalQuestion2ApiRequest.GetUrl)))
                .ReturnsAsync(additionalQuestion2ApiResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.CandidateDetails.Address.Should().BeEquivalentTo(addressApiResponse, options => options.Excluding(fil => fil.CandidateId));
            result.CandidateDetails.Should().BeEquivalentTo(candidateApiResponse, options=> options
                    .Excluding(p=>p.MiddleNames)
                    .Excluding(p=>p.DateOfBirth)
                    .Excluding(p=>p.Status)
                );
            result.IsApplicationComplete.Should().BeTrue();
        }
    }
}
