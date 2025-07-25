using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetApplicationQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetApplicationQuery query,
            GetApplicationApiResponse applicationApiResponse,
            GetApprenticeshipVacancyItemResponse vacancyApiResponse,
            GetQualificationReferenceTypesApiResponse qualificationReferenceTypesApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IVacancyService> vacancyService,
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

            vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.CandidateDetails.Address.Should().BeEquivalentTo(applicationApiResponse.Candidate.Address, options => options.Excluding(fil => fil.CandidateId).Excluding(c=>c.Uprn));
            result.CandidateDetails.Should().BeEquivalentTo(applicationApiResponse.Candidate, options=> options
                    .Excluding(p=>p.MiddleNames)
                    .Excluding(p=>p.DateOfBirth)
                    .Excluding(p=>p.Status)
                    .Excluding(p=>p.Address)
                    .Excluding(p=>p.MigratedEmail)
                );
            result.IsApplicationComplete.Should().BeTrue();
            result.EmploymentLocation.Should().BeEquivalentTo(applicationApiResponse.EmploymentLocation);
            result.ClosedDate.Should().Be(vacancyApiResponse.ClosedDate);
            result.ClosingDate.Should().Be(vacancyApiResponse.ClosingDate);
            result.VacancyTitle.Should().Be(vacancyApiResponse.Title);
            result.EmployerName.Should().Be(vacancyApiResponse.EmployerName);
        }
    }
}
