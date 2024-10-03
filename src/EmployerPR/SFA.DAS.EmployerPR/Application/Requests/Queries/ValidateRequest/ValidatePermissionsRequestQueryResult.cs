using SFA.DAS.EmployerPR.Common;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;

public class ValidatePermissionsRequestQueryResult
{
    public RequestStatus Status { get; set; }
    public bool? HasEmployerAccount { get; set; }
    public bool? HasValidaPaye { get; set; }
}
