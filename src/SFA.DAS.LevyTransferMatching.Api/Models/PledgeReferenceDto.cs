using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class PledgeReferenceDto
    {
        public string EncodedPledgeId { get; set; }


        public static implicit operator PledgeReferenceDto(PledgeReference pledgeReference)
        {
            return new PledgeReferenceDto()
            {
                EncodedPledgeId = pledgeReference.EncodedPledgeId,
            };
        }
    }
}