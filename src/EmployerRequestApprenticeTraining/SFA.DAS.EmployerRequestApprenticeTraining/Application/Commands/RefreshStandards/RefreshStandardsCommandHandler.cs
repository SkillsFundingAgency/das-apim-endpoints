using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.RefreshStandards
{
    public class RefreshStandardsCommandHandler : IRequestHandler<RefreshStandardsCommand, Unit>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public RefreshStandardsCommandHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
    }

        public async Task<Unit> Handle(RefreshStandardsCommand command, CancellationToken cancellationToken)
        {
            var request = new RefreshStandardsRequest(new RefreshStandardsData
            {
                Standards = command.Standards.Select(s => new StandardData
                { 
                    StandardLevel = s.StandardLevel,
                    StandardReference = s.StandardReference,
                    StandardSector = s.StandardSector,
                    StandardTitle = s.StandardTitle 
                }).ToList()
            }); ;

            await _requestApprenticeTrainingApiClient.Put(request);

            return Unit.Value;
        }
    }
}
