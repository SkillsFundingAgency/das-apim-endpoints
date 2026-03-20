using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using Milestone = SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone;
using SourceMilestone = SFA.DAS.LearnerData.Requests.Milestone;

namespace SFA.DAS.LearnerData.Services.ShortCourses
{
    public interface ICreateDraftShortCoursePostRequestBuilder
    {
        CreateDraftShortCourseRequest Build(ShortCourseRequest request, long ukprn);
    }

    public class CreateDraftShortCoursePostRequestBuilder(ILogger<CreateDraftShortCoursePostRequestBuilder> logger) : ICreateDraftShortCoursePostRequestBuilder
    {
        public CreateDraftShortCourseRequest Build(ShortCourseRequest request, long ukprn)
        {
            if (request.Delivery.OnProgramme.Count > 1)
            {
                logger.LogWarning("Multiple OnProgramme elements supplied for ShortCourse. Element with earliest StartDate will be processed; subsequent will be ignored");
            }

            var firstOnProg = request.Delivery.OnProgramme.MinBy(x => x.StartDate);

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
