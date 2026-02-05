using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;

namespace SFA.DAS.LearnerData.Services.ShortCourses
{
    public interface IUpdateLearningPostRequestBuilder
    {
        CreateDraftShortCourseApiPostRequest Build(ShortCourseRequest request);
    }

    public class UpdateLearningPostRequestBuilder : IUpdateLearningPostRequestBuilder
    {
        public CreateDraftShortCourseApiPostRequest Build(ShortCourseRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
