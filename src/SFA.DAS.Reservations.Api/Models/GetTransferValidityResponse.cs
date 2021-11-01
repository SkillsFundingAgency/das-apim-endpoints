using SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity;

namespace SFA.DAS.Reservations.Api.Models
{
    public class GetTransferValidityResponse
    {
        public bool IsValid { get; private set; }

        public static implicit operator GetTransferValidityResponse(GetTransferValidityQueryResult source)
        {
            return new GetTransferValidityResponse{ IsValid =  source.IsValid };
        }
    }
}
