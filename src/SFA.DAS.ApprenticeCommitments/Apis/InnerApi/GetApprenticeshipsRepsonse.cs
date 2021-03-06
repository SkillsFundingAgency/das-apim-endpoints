using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{

    public class GetApprenticeshipsRepsonse
    {
        public IEnumerable<long> Apprenticeships { get; set; }
    }
}