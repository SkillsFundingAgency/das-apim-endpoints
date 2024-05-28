using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommand : IRequest<CreateEmployerRequestResponse>
    {
        public RequestType RequestType { get; set; }
    }
}
