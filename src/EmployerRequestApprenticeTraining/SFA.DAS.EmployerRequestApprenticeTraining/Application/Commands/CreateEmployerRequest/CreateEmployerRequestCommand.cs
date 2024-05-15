using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Enums;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommand : IRequest<CreateEmployerRequestResponse>
    {
        public RequestType RequestType { get; set; }
    }
}
