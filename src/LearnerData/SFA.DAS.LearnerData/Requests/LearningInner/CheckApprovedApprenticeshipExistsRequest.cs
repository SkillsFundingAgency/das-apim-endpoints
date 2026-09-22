using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class CheckApprovedApprenticeshipExistsRequest : IHeadApiRequest
{
    public long Ukprn { get; set; }
    public string Uln { get; set; }
    public string TrainingCode { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsApproved { get; set; }

    public string HeadUrl =>
        $"{Ukprn}/apprenticeships?uln={Uln}&trainingCode={TrainingCode}&startDate={StartDate:yyyy-MM-dd}&isApproved={IsApproved}";
}
