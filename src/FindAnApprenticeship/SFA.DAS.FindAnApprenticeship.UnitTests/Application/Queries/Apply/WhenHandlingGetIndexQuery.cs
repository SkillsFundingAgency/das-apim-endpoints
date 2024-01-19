using System.Net;
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
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetIndexQuery query,
            GetApprenticeshipVacancyItemResponse faaApiResponse,
            PutApplicationApiResponse candidateApiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetIndexQueryHandler handler)
        {
            var expectedPutData = new PutApplicationApiRequest.PutApplicationApiRequestData
                { Email = query.ApplicantEmailAddress };
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

            using var scope = new AssertionScope();
            result.VacancyTitle.Should().Be(faaApiResponse.Title);
            result.EmployerName.Should().Be(faaApiResponse.EmployerName);
            result.ClosingDate.Should().Be(faaApiResponse.ClosingDate);
            result.IsDisabilityConfident.Should().Be(faaApiResponse.IsDisabilityConfident);
            result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(faaApiResponse.AdditionalQuestion1);
            result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(faaApiResponse.AdditionalQuestion2);

            result.EducationHistory.Qualifications.Should().Be(candidateApiResponse.QualificationStatus);
            result.EducationHistory.TrainingCourses.Should().Be(candidateApiResponse.TrainingCourseStatus);
            result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(candidateApiResponse.WorkExperienceStatus);
            result.WorkHistory.Jobs.Should().Be(candidateApiResponse.JobStatus);
            result.ApplicationQuestions.AdditionalQuestion1.Should().Be(candidateApiResponse.AdditionalQuestion1Status);
            result.ApplicationQuestions.AdditionalQuestion2.Should().Be(candidateApiResponse.AdditionalQuestion2Status);
            result.InterviewAdjustments.RequestAdjustments.Should().Be(candidateApiResponse.InterviewAdjustmentsStatus);
            result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(candidateApiResponse.DisabilityConfidenceStatus);
        }
    }
}
