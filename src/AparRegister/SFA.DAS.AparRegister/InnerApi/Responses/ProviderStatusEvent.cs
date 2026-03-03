using System;

namespace SFA.DAS.AparRegister.InnerApi.Responses;

public record ProviderStatusEvent(long Id, int ProviderId, string Event, DateTime CreatedOn);
