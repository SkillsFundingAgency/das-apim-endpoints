using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerRegistration;

public record AcknowledgeTrainingProviderTaskRequest(AcknowledgeTrainingProviderTaskData Data) : IPatchApiRequest<AcknowledgeTrainingProviderTaskData>
{
    public AcknowledgeTrainingProviderTaskData Data { get; set; } = Data;
    public string PatchUrl => "api/acknowledge-training-provider-task";
}

public record AcknowledgeTrainingProviderTaskData(long AccountId);