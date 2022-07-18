using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId
{
    public class GetAndAddEmployerVendorIdCommand : IRequest
    {
        public string HashedLegalEntityId { get; }

        public GetAndAddEmployerVendorIdCommand(string hashedLegalEntityId)
        {
            HashedLegalEntityId = hashedLegalEntityId;
        }
    }
}