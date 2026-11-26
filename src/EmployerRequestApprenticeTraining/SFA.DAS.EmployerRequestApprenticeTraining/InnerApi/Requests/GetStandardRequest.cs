using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        public string StandardReference { get; set; }
        
        public GetStandardRequest(string standardReference)
        {
            StandardReference = standardReference;
        }

        public string GetUrl => $"api/standards/{StandardReference}";
    }
}
