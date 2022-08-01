using AutoMapper;
using ResultTeamMember = SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications.TeamMember;
using InnerApiResponseTeamMember = SFA.DAS.SharedOuterApi.InnerApi.Responses.TeamMember;

namespace SFA.DAS.EmployerFinance.Mapping
{
	public class EmployerAccountProfile : Profile
	{
		public EmployerAccountProfile()
		{
			CreateMap<InnerApiResponseTeamMember, ResultTeamMember>();
		}
	}
}
