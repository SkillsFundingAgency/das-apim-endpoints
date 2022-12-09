using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    //MFCMFC this might go
    public class GetShortlistForUserResponse
    {
        public IEnumerable<GetShortlistItem> Shortlist { get; set; } = new List<GetShortlistItem>();
    }
}