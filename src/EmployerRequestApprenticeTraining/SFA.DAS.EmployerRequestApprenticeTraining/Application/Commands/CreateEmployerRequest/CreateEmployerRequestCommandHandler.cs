using MediatR;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Identity.Client;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommandHandler : IRequestHandler<CreateEmployerRequestCommand, CreateEmployerRequestResponse>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public CreateEmployerRequestCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<CreateEmployerRequestResponse> Handle(CreateEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateEmployerRequestRequest(new CreateEmployerRequestData
            {
                OriginalLocation = command.OriginalLocation,
                RequestType = command.RequestType,
                AccountId = command.AccountId,
                StandardReference = command.StandardReference,
                NumberOfApprentices = command.NumberOfApprentices,
                SingleLocation = command.SingleLocation,
                SingleLocationLatitude = command.SingleLocationLatitude,
                SingleLocationLongitude = command.SingleLocationLongitude,
                AtApprenticesWorkplace = command.AtApprenticesWorkplace,
                DayRelease = command.DayRelease,
                BlockRelease = command.BlockRelease,
                RequestedBy = command.RequestedBy,
                ModifiedBy = command.ModifiedBy
            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<CreateEmployerRequestData, CreateEmployerRequestResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
