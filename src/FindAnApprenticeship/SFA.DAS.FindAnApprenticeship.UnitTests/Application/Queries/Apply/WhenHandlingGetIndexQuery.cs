using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;

[TestFixture]
public class WhenHandlingGetIndexQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetIndexQuery query,
        Question questionOne,
        Question questionTwo,
        GetApplicationApiResponse applicationApiResponse,
        GetApplicationApiResponse previousApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyApiResponse,
        GetApprenticeshipVacancyItemResponse previousVacancyApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetIndexQueryHandler handler)
    {
        questionOne.QuestionOrder = 1;
        questionTwo.QuestionOrder = 2;
        applicationApiResponse.AdditionalQuestions = [questionOne, questionTwo];
            
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetPreviousApplicationRequest = new GetApplicationApiRequest(query.CandidateId, applicationApiResponse.PreviousAnswersSourceId!.Value, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetPreviousApplicationRequest.GetUrl)))
            .ReturnsAsync(previousApplicationApiResponse);
        vacancyService.Setup(x=>x.GetVacancy(previousApplicationApiResponse.VacancyReference)).ReturnsAsync(previousVacancyApiResponse);
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.VacancyTitle.Should().Be(vacancyApiResponse.Title);
        result.EmployerName.Should().Be(vacancyApiResponse.EmployerName);
        result.ClosingDate.Should().Be(vacancyApiResponse.ClosingDate);
        result.IsDisabilityConfident.Should().Be(vacancyApiResponse.IsDisabilityConfident);
        result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(questionOne.QuestionText);
        result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(questionTwo.QuestionText);

        result.EducationHistory.Qualifications.Should().Be(applicationApiResponse.QualificationsStatus);
        result.EducationHistory.TrainingCourses.Should().Be(applicationApiResponse.TrainingCoursesStatus);

        result.EmploymentLocation.Should().BeEquivalentTo(applicationApiResponse.EmploymentLocation);

        result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(applicationApiResponse.WorkExperienceStatus);
        result.WorkHistory.Jobs.Should().Be(applicationApiResponse.JobsStatus);
        result.ApplicationQuestions.AdditionalQuestion1.Should().Be(applicationApiResponse.AdditionalQuestion1Status);
        result.ApplicationQuestions.AdditionalQuestion2.Should().Be(applicationApiResponse.AdditionalQuestion2Status);
        result.ApplicationQuestions.AdditionalQuestion1Id.Should().Be(questionOne.Id);
        result.ApplicationQuestions.AdditionalQuestion2Id.Should().Be(questionTwo.Id);
        result.InterviewAdjustments.RequestAdjustments.Should().Be(applicationApiResponse.InterviewAdjustmentsStatus);
        
        result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(applicationApiResponse.DisabilityConfidenceStatus);
        result.PreviousApplication.EmployerName.Should().Be(previousVacancyApiResponse.EmployerName);
        result.PreviousApplication.VacancyTitle.Should().Be(previousVacancyApiResponse.Title);
        result.PreviousApplication.SubmissionDate.Should().Be(previousApplicationApiResponse.SubmittedDate);
        result.EmployerLocationOption.Should().Be(vacancyApiResponse.EmployerLocationOption);
        result.Address.Should().BeEquivalentTo(vacancyApiResponse.Address);
        result.OtherAddresses.Should().BeEquivalentTo(vacancyApiResponse.OtherAddresses);
    }
    [Test, MoqAutoData]
    public async Task Then_If_The_Previous_Vacancy_Is_Not_Found_Then_Null_Returned_In_Response(
        GetIndexQuery query,
        GetApplicationApiResponse applicationApiResponse,
        GetApplicationApiResponse previousApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetIndexQueryHandler handler)
    {

        applicationApiResponse.AdditionalQuestions = [];
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetPreviousApplicationRequest = new GetApplicationApiRequest(query.CandidateId, applicationApiResponse.PreviousAnswersSourceId!.Value, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetPreviousApplicationRequest.GetUrl)))
            .ReturnsAsync(previousApplicationApiResponse);

        vacancyService
            .Setup(client => client.GetVacancy(applicationApiResponse.VacancyReference.ToString()))
            .ReturnsAsync(vacancyApiResponse);

        vacancyService.Setup(x => x.GetVacancy(previousApplicationApiResponse.VacancyReference))
            .ReturnsAsync((IVacancy)null!);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.VacancyTitle.Should().Be(vacancyApiResponse.Title);
        result.EmployerName.Should().Be(vacancyApiResponse.EmployerName);
        result.ClosingDate.Should().Be(vacancyApiResponse.ClosingDate);
        result.IsDisabilityConfident.Should().Be(vacancyApiResponse.IsDisabilityConfident);
        result.ApplicationQuestions.AdditionalQuestion1Label.Should().BeNull();
        result.ApplicationQuestions.AdditionalQuestion2Label.Should().BeNull();

        result.EducationHistory.Qualifications.Should().Be(applicationApiResponse.QualificationsStatus);
        result.EducationHistory.TrainingCourses.Should().Be(applicationApiResponse.TrainingCoursesStatus);

        result.EmploymentLocation.Should().BeEquivalentTo(applicationApiResponse.EmploymentLocation);

        result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(applicationApiResponse.WorkExperienceStatus);
        result.WorkHistory.Jobs.Should().Be(applicationApiResponse.JobsStatus);
        result.ApplicationQuestions.AdditionalQuestion1.Should().Be(applicationApiResponse.AdditionalQuestion1Status);
        result.ApplicationQuestions.AdditionalQuestion2.Should().Be(applicationApiResponse.AdditionalQuestion2Status);
        result.ApplicationQuestions.AdditionalQuestion1Id.Should().BeNull();
        result.ApplicationQuestions.AdditionalQuestion2Id.Should().BeNull();
        result.InterviewAdjustments.RequestAdjustments.Should().Be(applicationApiResponse.InterviewAdjustmentsStatus);
        result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(applicationApiResponse.DisabilityConfidenceStatus);
        result.PreviousApplication.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Only_Question_Two_First_Question_Is_Not_Populated(GetIndexQuery query,
        Question questionTwo,
        GetApplicationApiResponse applicationApiResponse,
        GetApplicationApiResponse previousApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyApiResponse,
        GetApprenticeshipVacancyItemResponse previousVacancyApiResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetIndexQueryHandler handler)
    {
        questionTwo.QuestionOrder = 2;
        applicationApiResponse.AdditionalQuestions = [questionTwo];
            
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetPreviousApplicationRequest = new GetApplicationApiRequest(query.CandidateId, applicationApiResponse.PreviousAnswersSourceId!.Value, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetPreviousApplicationRequest.GetUrl)))
            .ReturnsAsync(previousApplicationApiResponse);
        vacancyService.Setup(x=>x.GetVacancy(previousApplicationApiResponse.VacancyReference)).ReturnsAsync(previousVacancyApiResponse);

        var expectedGetVacancyRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference.ToString());
        faaApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .ReturnsAsync(vacancyApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.ApplicationQuestions.AdditionalQuestion1Label.Should().BeNull();
        result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(questionTwo.QuestionText);
        result.ApplicationQuestions.AdditionalQuestion1Id.Should().BeNull();
        result.ApplicationQuestions.AdditionalQuestion2Id.Should().Be(questionTwo.Id);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Only_Question_Two_But_No_Order_First_Question_Is_Populated(GetIndexQuery query,
        Question questionTwo,
        GetApplicationApiResponse applicationApiResponse,
        GetApplicationApiResponse previousApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyApiResponse,
        GetApprenticeshipVacancyItemResponse previousVacancyApiResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetIndexQueryHandler handler)
    {
        questionTwo.QuestionOrder = null;
        applicationApiResponse.AdditionalQuestions = [questionTwo];
            
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetPreviousApplicationRequest = new GetApplicationApiRequest(query.CandidateId, applicationApiResponse.PreviousAnswersSourceId!.Value, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetPreviousApplicationRequest.GetUrl)))
            .ReturnsAsync(previousApplicationApiResponse);
        vacancyService.Setup(x=>x.GetVacancy(previousApplicationApiResponse.VacancyReference)).ReturnsAsync(previousVacancyApiResponse);

        var expectedGetVacancyRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference.ToString());
        faaApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .ReturnsAsync(vacancyApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        
        result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(questionTwo.QuestionText);
        result.ApplicationQuestions.AdditionalQuestion2Label.Should().BeNull();
        result.ApplicationQuestions.AdditionalQuestion1Id.Should().Be(questionTwo.Id);
        result.ApplicationQuestions.AdditionalQuestion2Id.Should().BeNull();
    }
     
    [Test, MoqAutoData]
    public async Task Then_If_Question_One_And_Two_But_No_Order_Questions_Are_Populated(GetIndexQuery query,
        Question questionOne,
        Question questionTwo,
        GetApplicationApiResponse applicationApiResponse,
        GetApplicationApiResponse previousApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyApiResponse,
        GetApprenticeshipVacancyItemResponse previousVacancyApiResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetIndexQueryHandler handler)
    {
        questionOne.QuestionOrder = null;
        questionTwo.QuestionOrder = null;
        applicationApiResponse.AdditionalQuestions = [questionOne,questionTwo];
            
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetPreviousApplicationRequest = new GetApplicationApiRequest(query.CandidateId, applicationApiResponse.PreviousAnswersSourceId!.Value, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetPreviousApplicationRequest.GetUrl)))
            .ReturnsAsync(previousApplicationApiResponse);
        vacancyService.Setup(x=>x.GetVacancy(previousApplicationApiResponse.VacancyReference)).ReturnsAsync(previousVacancyApiResponse);

        var expectedGetVacancyRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference.ToString());
        faaApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .ReturnsAsync(vacancyApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        
        result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(questionOne.QuestionText);
        result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(questionTwo.QuestionText);
        result.ApplicationQuestions.AdditionalQuestion1Id.Should().Be(questionOne.Id);
        result.ApplicationQuestions.AdditionalQuestion2Id.Should().Be(questionTwo.Id);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Closed_QueryResult_Is_Returned_As_Expected(
        GetIndexQuery query,
        Question questionOne,
        Question questionTwo,
        GetApplicationApiResponse applicationApiResponse,
        GetApplicationApiResponse previousApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyApiResponse,
        GetApprenticeshipVacancyItemResponse previousVacancyApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetIndexQueryHandler handler)
    {
        questionOne.QuestionOrder = 1;
        questionTwo.QuestionOrder = 2;
        applicationApiResponse.AdditionalQuestions = [questionOne, questionTwo];

        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetPreviousApplicationRequest = new GetApplicationApiRequest(query.CandidateId, applicationApiResponse.PreviousAnswersSourceId!.Value, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetPreviousApplicationRequest.GetUrl)))
            .ReturnsAsync(previousApplicationApiResponse);
        vacancyService.Setup(x => x.GetVacancy(previousApplicationApiResponse.VacancyReference)).ReturnsAsync(previousVacancyApiResponse);
        
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);
        vacancyService.Setup(x => x.GetClosedVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.VacancyTitle.Should().Be(vacancyApiResponse.Title);
        result.EmployerName.Should().Be(vacancyApiResponse.EmployerName);
        result.ClosingDate.Should().Be(vacancyApiResponse.ClosingDate);
        result.ClosedDate.Should().Be(vacancyApiResponse.ClosedDate);
        result.IsDisabilityConfident.Should().Be(vacancyApiResponse.IsDisabilityConfident);
        result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(questionOne.QuestionText);
        result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(questionTwo.QuestionText);

        result.EducationHistory.Qualifications.Should().Be(applicationApiResponse.QualificationsStatus);
        result.EducationHistory.TrainingCourses.Should().Be(applicationApiResponse.TrainingCoursesStatus);
        result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(applicationApiResponse.WorkExperienceStatus);
        result.WorkHistory.Jobs.Should().Be(applicationApiResponse.JobsStatus);
        result.ApplicationQuestions.AdditionalQuestion1.Should().Be(applicationApiResponse.AdditionalQuestion1Status);
        result.ApplicationQuestions.AdditionalQuestion2.Should().Be(applicationApiResponse.AdditionalQuestion2Status);
        result.ApplicationQuestions.AdditionalQuestion1Id.Should().Be(questionOne.Id);
        result.ApplicationQuestions.AdditionalQuestion2Id.Should().Be(questionTwo.Id);
        result.InterviewAdjustments.RequestAdjustments.Should().Be(applicationApiResponse.InterviewAdjustmentsStatus);
        result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(applicationApiResponse.DisabilityConfidenceStatus);
        result.PreviousApplication.EmployerName.Should().Be(previousVacancyApiResponse.EmployerName);
        result.PreviousApplication.VacancyTitle.Should().Be(previousVacancyApiResponse.Title);
        result.PreviousApplication.SubmissionDate.Should().Be(previousApplicationApiResponse.SubmittedDate);
    }
}