using MediatR;
using static SFA.DAS.EmployerRequestApprenticeTraining.Models.Enums;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommand : IRequest<CreateEmployerRequestResponse>
    {
        public RequestType RequestType { get; set; }
    }
}
