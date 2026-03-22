using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LearnerData.ShortCourses;
using Milestone = SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone;
using SourceMilestone = SFA.DAS.LearnerData.Requests.Milestone;

namespace SFA.DAS.LearnerData.Services.ShortCourses
{
    public interface ICreateDraftShortCoursePostRequestBuilder
    {
        CreateDraftShortCourseRequest Build(ShortCourseRequest request, long ukprn);
    }

    public class CreateDraftShortCoursePostRequestBuilder : ICreateDraftShortCoursePostRequestBuilder
    {
        public CreateDraftShortCourseRequest Build(ShortCourseRequest request, long ukprn)
        {
            var firstOnProg = request.Delivery.OnProgramme.First();

            var milestones = firstOnProg.Milestones
                .Select(m =>
                {
                    if (Enum.TryParse<Milestone>(m.ToString(), out var milestone)) return milestone;
                    throw new InvalidOperationException($"Invalid milestone value: {m}");
                })
                .ToList();

            if (firstOnProg.CompletionDate.HasValue && !firstOnProg.Milestones.Contains(SourceMilestone.LearningComplete))
                milestones.Add(Milestone.LearningComplete);

            return new CreateDraftShortCourseRequest
            {
                LearnerUpdateDetails = new ShortCourseLearningUpdateDetails
                {
                    Uln = request.Learner.Uln,
                    FirstName = request.Learner.FirstName,
                    LastName = request.Learner.LastName,
                    DateOfBirth = request.Learner.Dob,
                    EmailAddress = request.Learner.Email
                },
                LearningSupport = firstOnProg.LearningSupport
                    .Select(ls => new LearningSupportUpdatedDetails
                    {
                        StartDate = ls.StartDate,
                        EndDate = ls.EndDate
                    })
                    .ToList(),
                OnProgramme = new OnProgramme
                {
                    CourseCode = firstOnProg.CourseCode,
                    Ukprn = ukprn,
                    StartDate = firstOnProg.StartDate,
                    ExpectedEndDate = firstOnProg.ExpectedEndDate,
                    CompletionDate = firstOnProg.CompletionDate,
                    WithdrawalDate = firstOnProg.WithdrawalDate,
                    Milestones = milestones,
                    Price = 1000
                }
            };
        }
    }
}
