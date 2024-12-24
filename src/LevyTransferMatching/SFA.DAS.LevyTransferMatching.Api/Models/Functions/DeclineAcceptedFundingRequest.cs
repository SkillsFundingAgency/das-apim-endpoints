using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions;

public class DeclineAcceptedFundingRequest
{
    [Required]
    public int ApplicationId { get; set; }
}
