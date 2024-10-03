using Contentful.Core.Models;
using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdAndGuidListQueryResult
    {
        public List<ApprenticeKsbProgressData> KSBProgresses { get; set; }
    }
}
