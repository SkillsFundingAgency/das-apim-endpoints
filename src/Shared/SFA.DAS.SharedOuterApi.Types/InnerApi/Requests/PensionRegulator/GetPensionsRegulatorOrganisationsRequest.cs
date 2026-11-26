using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PensionRegulator;

public class GetPensionsRegulatorOrganisationsRequest(string aorn, string payeRef) : IGetApiRequest
{
    public string Aorn { get; } = aorn;
    public string PayeRef { get; } = Uri.EscapeDataString(payeRef);

    public string GetUrl => $"/api/PensionsRegulator/organisations/{Aorn}?payeRef={PayeRef}";
}