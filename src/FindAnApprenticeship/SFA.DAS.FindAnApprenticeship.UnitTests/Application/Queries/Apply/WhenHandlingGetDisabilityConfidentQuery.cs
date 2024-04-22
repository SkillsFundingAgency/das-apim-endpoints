using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetDisabilityConfidentQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(GetDisabilityConfidentQuery query,
            GetApplicationApiResponse applicationApiResponse,
            GetApprenticeshipVacancyItemResponse vacancyApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            GetDisabilityConfidentQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);

            candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);
            
            var expectedVacancyApiRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference);

            faaApiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                    It.Is<GetVacancyRequest>(r => r.GetUrl == expectedVacancyApiRequest.GetUrl)))
                .ReturnsAsync(vacancyApiResponse);

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
                vacancyApiResponse.EmployerName,
                applicationApiResponse.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = expectSectionCompleted
            });
        }
    }
}
