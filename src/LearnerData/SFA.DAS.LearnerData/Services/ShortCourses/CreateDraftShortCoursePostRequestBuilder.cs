using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using Milestone = SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone;

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
                    EmployerId = long.Parse(firstOnProg.AgreementId),
                    Ukprn = ukprn,
                    StartDate = firstOnProg.StartDate,
                    ExpectedEndDate = firstOnProg.ExpectedEndDate,
                    CompletionDate = firstOnProg.CompletionDate,
                    WithdrawalDate = firstOnProg.WithdrawalDate,
                    Milestones = firstOnProg.Milestones
                        .Select(m =>
                        {
                            if (Enum.TryParse<Milestone>(m.ToString(), out var milestone)) return milestone;
                            throw new InvalidOperationException($"Invalid milestone value: {m}");
                        })
                        .ToList(),
                    Price = 1000
                }
            };
        }
    }
}
