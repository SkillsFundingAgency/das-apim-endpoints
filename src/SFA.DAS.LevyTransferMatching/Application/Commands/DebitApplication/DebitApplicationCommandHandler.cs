using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication
{
    public class DebitApplicationCommandHandler : IRequestHandler<DebitApplicationCommand, DebitApplicationCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public DebitApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<DebitApplicationCommandResult> Handle(DebitApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));
            var standard = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(application.StandardId));

            var data = new DebitApplicationRequest.DebitApplicationRequestData
            {
                NumberOfApprentices = request.NumberOfApprentices,
                Amount = request.Amount,
                MaxAmount = standard.MaxFunding * application.NumberOfApprentices
            };

            var response = await _levyTransferMatchingService.DebitApplication(new DebitApplicationRequest(request.ApplicationId, data));

            return new DebitApplicationCommandResult
            {
                ErrorContent = response.ErrorContent,
                StatusCode = response.StatusCode
            };
        }
    }
}
