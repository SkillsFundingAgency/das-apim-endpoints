using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface ICreateUnapprovedShortCourseLearningRequestBuilder
{
    CreateUnapprovedShortCourseLearningRequest Build(ShortCourseRequest request, ShortCourseOnProgramme onProg, Guid learningKey, Guid episodeKey, long ukprn, SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme resolvedOnProgramme);
}

public class CreateUnapprovedShortCourseLearningRequestBuilder : ICreateUnapprovedShortCourseLearningRequestBuilder
{
    public CreateUnapprovedShortCourseLearningRequest Build(ShortCourseRequest request, ShortCourseOnProgramme onProg, Guid learningKey, Guid episodeKey, long ukprn, SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme resolvedOnProgramme)
    {
        var milestones = onProg.Milestones.Select(x =>
            x == Milestone.LearningComplete
                ? Milestone.LearningComplete
                : Milestone.ThirtyPercentLearningComplete).ToList();

        if (onProg.CompletionDate.HasValue && !onProg.Milestones.Contains(Milestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        return new CreateUnapprovedShortCourseLearningRequest
        {
            LearningKey = learningKey,
            EpisodeKey = episodeKey,
            Learner = new Learner
            {
                DateOfBirth = request.Learner.Dob,
                Uln = request.Learner.Uln.ToString()
            },
            LearningSupport = onProg.LearningSupport.Select(x => new LearningSupport
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList(),
            OnProgramme = new Requests.EarningsInner.OnProgramme
            {
                StartDate = onProg.StartDate,
                CompletionDate = onProg.CompletionDate,
                CourseCode = onProg.CourseCode,
                ExpectedEndDate = onProg.ExpectedEndDate,
                Milestones = milestones,
                TotalPrice = resolvedOnProgramme.Price,
                LearningType = resolvedOnProgramme.LearningType,
                Ukprn = ukprn,
                WithdrawalDate = onProg.WithdrawalDate
            }
        };
    }
}
