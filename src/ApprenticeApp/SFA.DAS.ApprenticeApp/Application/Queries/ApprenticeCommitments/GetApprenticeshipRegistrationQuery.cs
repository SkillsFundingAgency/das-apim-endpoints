using MediatR;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration
{
    public class GetApprenticeshipRegistrationQuery : IRequest<GetApprenticeshipRegistrationQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}