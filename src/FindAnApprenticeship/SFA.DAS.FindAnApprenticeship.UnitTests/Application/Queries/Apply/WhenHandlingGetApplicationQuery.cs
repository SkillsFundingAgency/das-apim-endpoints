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
            GetTrainingCoursesApiResponse trainingCoursesApiResponse,
            GetWorkHistoriesApiResponse jobWorkHistoriesApiResponse,
            GetWorkHistoriesApiResponse volunteeringWorkHistoriesApiResponse,
            GetAboutYouItemApiResponse aboutYouItemApiResponse,
            GetAdditionalQuestionApiResponse additionalQuestion1ApiResponse,
            GetAdditionalQuestionApiResponse additionalQuestion2ApiResponse,
            GetApprenticeshipVacancyItemResponse apprenticeshipVacancyItemApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            GetApplicationQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);
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

            var expectedGetTrainingCoursesApiRequest = new GetTrainingCoursesApiRequest(query.ApplicationId, query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetTrainingCoursesApiResponse>(
                    It.Is<GetTrainingCoursesApiRequest>(r => r.GetUrl == expectedGetTrainingCoursesApiRequest.GetUrl)))
                .ReturnsAsync(trainingCoursesApiResponse);

            var expectedGetWorkHistoriesApiRequest = new GetWorkHistoriesApiRequest(query.ApplicationId, query.CandidateId, WorkHistoryType.Job);
            candidateApiClient
                .Setup(client => client.Get<GetWorkHistoriesApiResponse>(
                    It.Is<GetWorkHistoriesApiRequest>(r => r.GetUrl == expectedGetWorkHistoriesApiRequest.GetUrl)))
                .ReturnsAsync(jobWorkHistoriesApiResponse);

            var expectedGetVolunteeringWorkHistoriesApiRequest = new GetWorkHistoriesApiRequest(query.ApplicationId, query.CandidateId, WorkHistoryType.WorkExperience);
            candidateApiClient
                .Setup(client => client.Get<GetWorkHistoriesApiResponse>(
                    It.Is<GetWorkHistoriesApiRequest>(r => r.GetUrl == expectedGetVolunteeringWorkHistoriesApiRequest.GetUrl)))
                .ReturnsAsync(volunteeringWorkHistoriesApiResponse);

            var expectedGetAboutYouItemApiRequest = new GetAboutYouItemApiRequest(query.ApplicationId, query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetAboutYouItemApiResponse>(
                    It.Is<GetAboutYouItemApiRequest>(r => r.GetUrl == expectedGetAboutYouItemApiRequest.GetUrl)))
                .ReturnsAsync(aboutYouItemApiResponse);

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
            result.CandidateDetails.Should().BeEquivalentTo(candidateApiResponse);
        }
    }
}
