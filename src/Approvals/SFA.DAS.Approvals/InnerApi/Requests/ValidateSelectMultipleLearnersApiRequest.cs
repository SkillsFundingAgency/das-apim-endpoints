using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Types;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class ValidateSelectMultipleLearnersApiRequest : SaveDataRequest
{
    public long ProviderId { get; set; }
    public IEnumerable<BulkUploadAddDraftApprenticeshipExtendedRequest> CsvRecords { get; set; }
    public BulkReservationValidationResults BulkReservationValidationResults { get; set; }
    public ProviderStandardsData ProviderStandardsData { get; set; }
    public Dictionary<string, int?> OtjTrainingHours { get; set; }
}