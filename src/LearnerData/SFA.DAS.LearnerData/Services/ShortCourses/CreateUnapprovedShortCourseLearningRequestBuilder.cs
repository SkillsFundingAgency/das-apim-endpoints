using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using Milestone = SFA.DAS.LearnerData.Requests.Milestone;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface ICreateUnapprovedShortCourseLearningRequestBuilder
{
    CreateUnapprovedShortCourseLearningRequest Build(ShortCourseRequest request, CreateShortCoursePostResponse learningResponse, long ukprn);
}

public class CreateUnapprovedShortCourseLearningRequestBuilder : ICreateUnapprovedShortCourseLearningRequestBuilder
{
    public CreateUnapprovedShortCourseLearningRequest Build(ShortCourseRequest request, CreateShortCoursePostResponse learningResponse, long ukprn)
    {
        var firstOnProg = request.Delivery.OnProgramme.First();

        return new CreateUnapprovedShortCourseLearningRequest
        {
            LearningKey = learningResponse.LearningKey,
            EpisodeKey = learningResponse.EpisodeKey,
            Learner = new Learner
            {
                DateOfBirth = request.Learner.Dob,
                Uln = request.Learner.Uln.ToString()
            },
            LearningSupport = firstOnProg.LearningSupport.Select(x => new LearningSupportItem
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList(),
            OnProgramme = new OnProgramme
            {
                StartDate = firstOnProg.StartDate,
                CompletionDate = firstOnProg.CompletionDate,
                CourseCode = firstOnProg.CourseCode,
                EmployerId = 0,
                ExpectedEndDate = firstOnProg.ExpectedEndDate,
                Milestones = firstOnProg.Milestones.Select(x =>
                    x == Milestone.LearningComplete
                        ? SharedOuterApi.InnerApi.Requests.Earnings.Milestone.LearningComplete
                        : SharedOuterApi.InnerApi.Requests.Earnings.Milestone.ThirtyPercentLearningComplete).ToList(),
                TotalPrice = 1000, //todo future story FLP-1530, default to 1000 until courses api ready
                Ukprn = ukprn,
                WithdrawalDate = firstOnProg.WithdrawalDate
            }
        };
    }
}