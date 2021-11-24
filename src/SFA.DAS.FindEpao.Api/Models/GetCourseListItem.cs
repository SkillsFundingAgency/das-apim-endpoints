using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseListItem
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public int Id { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        

        public static implicit operator GetCourseListItem(GetStandardsListItem standard)
        {
            return new GetCourseListItem
            {
                Id = standard.LarsCode,
                Level = standard.Level,
                Title = standard.Title,
                IntegratedApprenticeship = standard.IntegratedApprenticeship
            };
        }
    }

    public class GetAllCoursesListItem : GetCourseListItem
    {
        public string[] StandardVersions { get; set; }

        public static implicit operator GetAllCoursesListItem(GetStandardsListItem standard)
        {
            return new GetAllCoursesListItem
            {
                Id = standard.LarsCode,
                IntegratedApprenticeship = standard.IntegratedApprenticeship,
                Level = standard.Level,
                Title = standard.Title,
                StandardVersions = standard.StandardVersions
            };
        }



    }
}