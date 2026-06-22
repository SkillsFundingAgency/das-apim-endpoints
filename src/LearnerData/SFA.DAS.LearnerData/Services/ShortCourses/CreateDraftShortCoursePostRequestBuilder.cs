using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Shared;
using OnProgramme = SFA.DAS.LearnerData.Requests.EarningsInner.OnProgramme;

namespace SFA.DAS.LearnerData.Services.ShortCourses
{
    public interface ICreateDraftShortCoursePostRequestBuilder
    {
        Task<CreateDraftShortCourseRequest> Build(ShortCourseRequest request, long ukprn);
    }

    public class CreateDraftShortCoursePostRequestBuilder(
        ILogger<CreateDraftShortCoursePostRequestBuilder> logger,
        IShortCourseLookupService shortCourseLookupService) : ICreateDraftShortCoursePostRequestBuilder
    {
        public async Task<CreateDraftShortCourseRequest> Build(ShortCourseRequest request, long ukprn)
        {
            var onProgrammeItems = new List<SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme>();

            foreach (var onProg in request.Delivery.OnProgramme)
            {
                var milestones = onProg.Milestones
                    .Select(m =>
                    {
                        if (Enum.TryParse<Milestone>(m.ToString(), out var milestone)) return milestone;
                        throw new InvalidOperationException($"Invalid milestone value: {m}");
                    })
                    .ToList();

                if (onProg.CompletionDate.HasValue && !onProg.Milestones.Contains(Milestone.LearningComplete))
                    milestones.Add(Milestone.LearningComplete);

                var courseDetails = await shortCourseLookupService.GetCourseDetails(onProg.CourseCode, onProg.StartDate);

                onProgrammeItems.Add(new SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme
                {
                    CourseCode = onProg.CourseCode,
                    Ukprn = ukprn,
                    StartDate = onProg.StartDate,
                    ExpectedEndDate = onProg.ExpectedEndDate,
                    CompletionDate = onProg.CompletionDate,
                    WithdrawalDate = onProg.WithdrawalDate,
                    WithdrawalReasonCode = onProg.WithdrawalReasonCode,
                    Milestones = milestones,
                    Price = courseDetails.Price,
                    LearningType = courseDetails.LearningType
                });
            }

            var firstOnProg = request.Delivery.OnProgramme.First();

            return new CreateDraftShortCourseRequest
            {
                LearnerUpdateDetails = new ShortCourseLearningUpdateDetails
                {
                    Uln = request.Learner.Uln,
                    FirstName = request.Learner.FirstName,
                    LastName = request.Learner.LastName,
                    DateOfBirth = request.Learner.Dob,
                    EmailAddress = request.Learner.Email,
                    LearnerRef = request.Learner.LearnerRef
                },
                LearningSupport = firstOnProg.LearningSupport,
                OnProgramme = onProgrammeItems
            };
        }
    }
}
