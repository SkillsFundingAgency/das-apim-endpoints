using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequestForResponseNotification
    {
        public Guid RequestedBy { get; set; }
        public long AccountId { get; set; }
        public List<StandardDetails> Standards { get; set; }
    }

    public class StandardDetails
    {
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }


    }
}
