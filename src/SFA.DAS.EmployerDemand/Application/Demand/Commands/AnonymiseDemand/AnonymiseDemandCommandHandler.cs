using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.AnonymiseDemand
{
    public class AnonymiseDemandCommandHandler : IRequestHandler<AnonymiseDemandCommand, Unit> 
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;

        public AnonymiseDemandCommandHandler(IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient)
        {
            _employerDemandApiClient = employerDemandApiClient;
        }

        public async Task<Unit> Handle(AnonymiseDemandCommand request, CancellationToken cancellationToken)
        {
            var employerDemand = await _employerDemandApiClient.Get<GetEmployerDemandResponse>(
                new GetEmployerDemandRequest(request.EmployerDemandId));

            await _employerDemandApiClient.PatchWithResponseCode(
                new PatchCourseDemandRequest(
                    request.EmployerDemandId,
                    new PatchCourseDemandData
                    {
                        EmailAddress = null
                    }));
            //todo: verify what happens if other values are not set; may need to patch all settable values

            return Unit.Value;
        }
    }
}

