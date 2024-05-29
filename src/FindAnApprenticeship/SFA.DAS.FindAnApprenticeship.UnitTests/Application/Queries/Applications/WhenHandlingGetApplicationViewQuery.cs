using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetApplicationViewQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetApplicationViewQuery query,
            GetApplicationApiResponse applicationApiResponse,
            IVacancy vacancyResponse,
            GetQualificationReferenceTypesApiResponse qualificationReferenceTypesApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IVacancyService> vacancyService,
            GetApplicationViewQueryHandler handler)
        {
            applicationApiResponse.VacancyReference = "1234567890";
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

            vacancyService.Setup(client => client.GetVacancy(applicationApiResponse.VacancyReference))
                .ReturnsAsync(vacancyResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.CandidateDetails.Address.Should().BeEquivalentTo(applicationApiResponse.Candidate.Address, options => options.Excluding(fil => fil.CandidateId));
            result.CandidateDetails.Should().BeEquivalentTo(applicationApiResponse.Candidate, options=> options
                    .Excluding(p=>p.MiddleNames)
                    .Excluding(p=>p.DateOfBirth)
                    .Excluding(p=>p.Status)
                    .Excluding(p=>p.Address)
                );
            result.VacancyDetails.EmployerName.Should().Be(vacancyResponse.EmployerName);
            result.VacancyDetails.Title.Should().Be(vacancyResponse.Title);
            result.ApplicationStatus.Should().Be(applicationApiResponse.Status);
            result.WithdrawnDate.Should().Be(applicationApiResponse.WithdrawnDate);
        }
    }
}
