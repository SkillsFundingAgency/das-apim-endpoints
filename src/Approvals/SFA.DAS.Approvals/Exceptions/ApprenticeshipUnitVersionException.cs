using Newtonsoft.Json;
using SFA.DAS.Approvals.Validation;

namespace SFA.DAS.Approvals.Exceptions;

public class ApprenticeshipUnitVersionException(string message)
    : DomainApimException(JsonConvert.SerializeObject(new ErrorResponse([new ErrorDetail("ActualStartDate", message)])))
{
    public override string Message => message;
}
