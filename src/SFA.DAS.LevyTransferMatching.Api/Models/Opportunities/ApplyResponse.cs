using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunities
{
    public class ApplyResponse
    {
        public int ApplicationId { get; set; }

        public static implicit operator ApplyResponse(CreateApplicationCommandResult commandResult)
        {
            return new ApplyResponse()
            {
                ApplicationId = commandResult.ApplicationId
            };
        }
    }
}
