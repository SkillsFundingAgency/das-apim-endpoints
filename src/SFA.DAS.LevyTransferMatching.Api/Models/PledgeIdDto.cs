namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class PledgeIdDto
    {
        public int Id { get; set; }

        public static implicit operator PledgeIdDto(int pledgeId)
        {
            return new PledgeIdDto
            {
                Id = pledgeId
            };
        }
    }
}