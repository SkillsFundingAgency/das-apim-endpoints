using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
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
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetIndexQuery query,
            GetApplicationApiResponse applicationApiResponse,
            GetApprenticeshipVacancyItemResponse vacancyApiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetIndexQueryHandler handler)
        {
            var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationApiResponse>(
                    It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var expectedGetVacancyRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference);
            faaApiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
                .ReturnsAsync(vacancyApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.VacancyTitle.Should().Be(vacancyApiResponse.Title);
            result.EmployerName.Should().Be(vacancyApiResponse.EmployerName);
            result.ClosingDate.Should().Be(vacancyApiResponse.ClosingDate);
            result.IsDisabilityConfident.Should().Be(vacancyApiResponse.IsDisabilityConfident);
            result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(vacancyApiResponse.AdditionalQuestion1);
            result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(vacancyApiResponse.AdditionalQuestion2);

            result.EducationHistory.Qualifications.Should().Be(applicationApiResponse.QualificationsStatus);
            result.EducationHistory.TrainingCourses.Should().Be(applicationApiResponse.TrainingCoursesStatus);
            result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(applicationApiResponse.WorkExperienceStatus);
            result.WorkHistory.Jobs.Should().Be(applicationApiResponse.JobsStatus);
            result.ApplicationQuestions.AdditionalQuestion1.Should().Be(applicationApiResponse.AdditionalQuestion1Status);
            result.ApplicationQuestions.AdditionalQuestion2.Should().Be(applicationApiResponse.AdditionalQuestion2Status);
            result.ApplicationQuestions.AdditionalQuestion1Id.Should().Be(applicationApiResponse.AdditionalQuestions[0].Id);
            result.ApplicationQuestions.AdditionalQuestion2Id.Should().Be(applicationApiResponse.AdditionalQuestions[1].Id);
            result.InterviewAdjustments.RequestAdjustments.Should().Be(applicationApiResponse.InterviewAdjustmentsStatus);
            result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(applicationApiResponse.DisabilityConfidenceStatus);
        }
    }
}
