using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;

public record GetStagedApprenticeRequest(string LastName, DateTime DateOfBirth, string Email) : IGetApiRequest
{
    public string GetUrl => $"stagedapprentices?lastName={LastName}&dateOfBirth={DateOfBirth.Date:yyyy-MM-dd}&email={Email}";
}
