using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using Milestone = SFA.DAS.LearnerData.Requests.Milestone;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface ICreateUnapprovedShortCourseLearningRequestBuilder
{
    CreateUnapprovedShortCourseLearningRequest Build(ShortCourseRequest request, Guid learningKey, Guid episodeKey, long ukprn, CreateDraftShortCourseRequest learningRequest);
}

public class CreateUnapprovedShortCourseLearningRequestBuilder : ICreateUnapprovedShortCourseLearningRequestBuilder
{
    public CreateUnapprovedShortCourseLearningRequest Build(ShortCourseRequest request, Guid learningKey, Guid episodeKey, long ukprn, CreateDraftShortCourseRequest learningRequest)
    {
        var firstOnProg = request.Delivery.OnProgramme.First();

        var milestones = firstOnProg.Milestones.Select(x =>
            x == Milestone.LearningComplete
                ? SFA.DAS.LearnerData.Requests.EarningsInner.Milestone.LearningComplete
                : SFA.DAS.LearnerData.Requests.EarningsInner.Milestone.ThirtyPercentLearningComplete).ToList();

        if (firstOnProg.CompletionDate.HasValue && !firstOnProg.Milestones.Contains(Milestone.LearningComplete))
            milestones.Add(SFA.DAS.LearnerData.Requests.EarningsInner.Milestone.LearningComplete);

        return new CreateUnapprovedShortCourseLearningRequest
        {
            LearningKey = learningKey,
            EpisodeKey = episodeKey,
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
            OnProgramme = new SFA.DAS.LearnerData.Requests.EarningsInner.OnProgramme
            {
                StartDate = firstOnProg.StartDate,
                CompletionDate = firstOnProg.CompletionDate,
                CourseCode = firstOnProg.CourseCode,
                ExpectedEndDate = firstOnProg.ExpectedEndDate,
                Milestones = milestones,
                TotalPrice = learningRequest.OnProgramme.Price,
                LearningType = learningRequest.OnProgramme.LearningType,
                Ukprn = ukprn,
                WithdrawalDate = firstOnProg.WithdrawalDate
            }
        };
    }
}
