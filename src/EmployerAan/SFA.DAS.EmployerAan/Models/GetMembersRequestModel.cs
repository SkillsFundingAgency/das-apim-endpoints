using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Models
{
    public class GetMembersRequestModel
    {
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
        public Guid RequestedByMemberId { get; set; }

        [FromQuery]
        public string? Keyword { get; set; }

        [FromQuery]
        public List<int> RegionId { get; set; } = new List<int>();

        [FromQuery]
        public List<MemberUserType> UserType { get; set; } = new List<MemberUserType>();

        [FromQuery]
        public List<MembershipStatusType> Status { get; set; } = new List<MembershipStatusType>();

        [FromQuery]
        public bool? IsRegionalChair { get; set; }

        [FromQuery]
        public int? Page { get; set; }

        [FromQuery]
        public int? PageSize { get; set; }

        public static implicit operator GetMembersQuery(GetMembersRequestModel model) => new()
        {
            RequestedByMemberId = model.RequestedByMemberId,
            Keyword = model.Keyword,
            RegionIds = model.RegionId,
            UserType = model.UserType,
            Status = model.Status,
            IsRegionalChair = model.IsRegionalChair,
            Page = model.Page,
            PageSize = model.PageSize
        };
    }
}
