using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseListItem
    {
        public string Title { get ; set ; }
        public int Level { get ; set ; }
        public int Id { get ; set ; }
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }
        public bool IntegratedApprenticeship { get ; set ; }

        public static implicit operator GetCourseListItem(GetStandardsListItem standard)
        {
            return new GetCourseListItem
            {
                Id = standard.Id,
                LarsCode = standard.LarsCode,
                StandardUId = standard.StandardUId,
                Level = standard.Level,
                Title = standard.Title,
                IntegratedApprenticeship = standard.IntegratedApprenticeship
            };
        }
    }
}