using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider
{
    public class AvailableCourseModel
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public static implicit operator AvailableCourseModel  (GetStandardResponse source) 
            => new AvailableCourseModel 
            { 
                LarsCode = source.LarsCode,
                Title = source.Title,
                Level = source.Level
            };
    }
}
