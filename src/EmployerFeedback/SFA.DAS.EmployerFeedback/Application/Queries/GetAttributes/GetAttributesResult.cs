using System.Collections.Generic;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes
{
    public class GetAttributesResult
    {
        public List<Attribute> Attributes { get; set; }
    }
}