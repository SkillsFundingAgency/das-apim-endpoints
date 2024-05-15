using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetProviderAccountLegalEntitiesRequest : IGetApiRequest
    {
        private readonly int? _ukprn;
        private List<Operation> _operations;

        public GetProviderAccountLegalEntitiesRequest(int? ukprn, List<Operation> operations)
        {
            _ukprn = ukprn;
            _operations = operations.Any() ? operations : new List<Operation>();
        }

        public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&operations={_operations}";
    }
}