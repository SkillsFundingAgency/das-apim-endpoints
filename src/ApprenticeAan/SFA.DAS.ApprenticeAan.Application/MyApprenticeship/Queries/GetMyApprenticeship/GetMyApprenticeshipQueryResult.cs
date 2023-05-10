using System.Net;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQueryResult
{
    public HttpStatusCode StatusCode { get; set; }
    public MyApprenticeship? MyApprenticeship { get; set; }
}