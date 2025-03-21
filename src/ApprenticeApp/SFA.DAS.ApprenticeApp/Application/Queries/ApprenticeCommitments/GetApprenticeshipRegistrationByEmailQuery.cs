using MediatR;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments
{
    public class GetApprenticeshipRegistrationByEmailQuery : IRequest<GetApprenticeshipRegistrationQueryResult>
    {
        public string Email { get; set; }
    }
}
