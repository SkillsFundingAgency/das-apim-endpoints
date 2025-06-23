using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public record RelatedOccupation(string Title, int Level)
{
    public static implicit operator RelatedOccupation(RelatedOccupationResponse standard)
    {
        return new RelatedOccupation(standard.Title, standard.Level);
    }
}