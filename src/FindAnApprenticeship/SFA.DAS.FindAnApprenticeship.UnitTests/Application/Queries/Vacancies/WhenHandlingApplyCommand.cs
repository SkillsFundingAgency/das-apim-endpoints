using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Vacancies;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Vacancies
{
    [TestFixture]
    public class WhenHandlingApplyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
           ApplyCommand query,
           GetApprenticeshipVacancyItemResponse faaApiResponse,
           PutApplicationApiResponse candidateApiResponse,
           [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
           [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
           ApplyCommandHandler handler)
        {
            var expectedPutData = new PutApplicationApiRequest.PutApplicationApiRequestData
            { CandidateId = query.CandidateId };
            var expectedPutRequest = new PutApplicationApiRequest(query.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase), expectedPutData);

            var expectedGetRequest = new GetVacancyRequest(query.VacancyReference);

            faaApiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetRequest.GetUrl)))
                .ReturnsAsync(faaApiResponse);

            candidateApiClient
                .Setup(client => client.PutWithResponseCode<PutApplicationApiResponse>(
                    It.Is<PutApplicationApiRequest>(r => r.PutUrl == expectedPutRequest.PutUrl)))
                .ReturnsAsync(new ApiResponse<PutApplicationApiResponse>(candidateApiResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.ApplicationId.Should().Be(candidateApiResponse.Id);
        }
    }
}
