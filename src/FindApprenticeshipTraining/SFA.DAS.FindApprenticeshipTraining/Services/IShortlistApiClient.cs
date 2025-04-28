using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Services
{
    [Obsolete("FAT25 Delete once all references have been removed")]
    public interface IShortlistApiClient<T> : IInternalApiClient<T>
    {
    }
}
