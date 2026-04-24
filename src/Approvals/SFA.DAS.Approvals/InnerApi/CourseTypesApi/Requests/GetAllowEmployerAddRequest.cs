using System;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;

public class GetAllowEmployerAddRequest(string learningType) : IGetApiRequest
{
    public string LearningType { get; } = learningType;
    public string GetUrl => $"api/coursetypes/features/allowEmployerAdd?learningType={Uri.EscapeDataString(LearningType)}";
}
