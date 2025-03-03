using MediatR;

namespace SFA.DAS.EmployerAan.Application.Onboarding.ConfirmDetails.Queries
{
    public class GetOnboardingConfirmDetailsQuery : IRequest<GetOnboardingConfirmDetailsQueryResult>
    {
        public long EmployerAccountId { get; set; }
    }
}
