using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ValidationOverride
{
    public class ValidationOverrideCommand : IRequest
    {   
        public ValidationOverrideRequest ValidationOverrideRequest { get; }

        public ValidationOverrideCommand(ValidationOverrideRequest validationOverrideRequest)
        {
            ValidationOverrideRequest = validationOverrideRequest;
        }
    }
}
