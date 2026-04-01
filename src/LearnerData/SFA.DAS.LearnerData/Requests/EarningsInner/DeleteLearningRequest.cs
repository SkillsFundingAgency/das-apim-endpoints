<<<<<<<< HEAD:src/Shared/SFA.DAS.SharedOuterApi.Types/InnerApi/Requests/Earnings/DeleteLearningRequest.cs
﻿using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Earnings;
========
﻿using SFA.DAS.SharedOuterApi.Interfaces;
>>>>>>>> origin/master:src/LearnerData/SFA.DAS.LearnerData/Requests/EarningsInner/DeleteLearningRequest.cs

namespace SFA.DAS.LearnerData.Requests.EarningsInner;
    
public class DeleteLearningRequest(Guid learningKey) : IDeleteApiRequest
{
    public Guid LearningKey { get; set; } = learningKey;
    public string DeleteUrl => $"learning/{LearningKey}";
}
