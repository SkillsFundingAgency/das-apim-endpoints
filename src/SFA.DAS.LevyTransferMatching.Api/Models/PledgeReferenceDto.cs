using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class PledgeReferenceDto
    {
        public int? Id { get; set; }


        public static implicit operator PledgeReferenceDto(PledgeReference pledgeReference)
        {
            return new PledgeReferenceDto()
            {
                Id = pledgeReference.Id,
            };
        }
    }
}