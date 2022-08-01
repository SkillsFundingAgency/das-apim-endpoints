using AutoMapper;
using ResultTeamMember = SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications.TeamMember;
using OuterApiResponseTeamMember = SFA.DAS.EmployerFinance.Api.Models.Accounts.TeamMember;

namespace SFA.DAS.EmployerFinance.Mapping
{
	public class EmployerAccountProfile : Profile
	{
		public EmployerAccountProfile()
		{
			CreateMap<ResultTeamMember, OuterApiResponseTeamMember>();
		}
	}
}
