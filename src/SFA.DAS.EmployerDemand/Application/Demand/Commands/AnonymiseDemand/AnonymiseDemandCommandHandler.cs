using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
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
            await _employerDemandApiClient.PatchWithResponseCode(
                new PatchCourseDemandRequest(
                    request.EmployerDemandId,
                    new PatchOperation
                    {
                        Path = "ContactEmailAddress",
                        Value = string.Empty
                    }));

            return Unit.Value;
        }
    }
}

