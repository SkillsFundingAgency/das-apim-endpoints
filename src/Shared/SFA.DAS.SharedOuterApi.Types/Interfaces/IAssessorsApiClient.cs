using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

[Obsolete("The misnamed Assessors api client is being replaced with Assessor during the rewrite of the Assessor service")]
public interface IAssessorsApiClient<T> : IInternalApiClient<T>;