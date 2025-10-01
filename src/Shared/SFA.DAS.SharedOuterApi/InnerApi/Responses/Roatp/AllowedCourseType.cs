using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public record AllowedCourseType(int CourseTypeId, string CourseTypeName, LearningType LearningType);
