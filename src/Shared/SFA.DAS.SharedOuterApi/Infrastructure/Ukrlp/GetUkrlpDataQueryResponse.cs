<<<<<<<< HEAD:src/Shared/SFA.DAS.SharedOuterApi.Types/Infrastructure/Ukrlp/GetUkrlpDataQueryResponse.cs

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.Ukrlp
========
﻿using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp
>>>>>>>> master:src/Shared/SFA.DAS.SharedOuterApi/Infrastructure/Ukrlp/GetUkrlpDataQueryResponse.cs
{
    public class GetUkrlpDataQueryResponse
    {
        public bool Success { get; set; }
        public List<ProviderAddress> Results { get; set; }
    }
}
