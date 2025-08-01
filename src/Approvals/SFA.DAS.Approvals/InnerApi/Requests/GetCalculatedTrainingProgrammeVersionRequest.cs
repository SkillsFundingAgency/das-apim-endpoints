using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetCalculatedTrainingProgrammeVersionRequest(int courseCode, DateTime startDate) : IGetApiRequest
{
    public string GetUrl => $"api/TrainingProgramme/calculate-version/{courseCode}?startDate={startDate.ToString("O", System.Globalization.CultureInfo.InvariantCulture)}";
}