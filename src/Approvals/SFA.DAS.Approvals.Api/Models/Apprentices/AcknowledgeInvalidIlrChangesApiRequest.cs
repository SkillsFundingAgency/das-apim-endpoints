using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class AcknowledgeInvalidIlrChangesApiRequest
{
    public UserInfo UserInfo { get; set; }
    public List<InvalidIlrChangeAcknowledgement> Acknowledgements { get; set; } = [];
}
