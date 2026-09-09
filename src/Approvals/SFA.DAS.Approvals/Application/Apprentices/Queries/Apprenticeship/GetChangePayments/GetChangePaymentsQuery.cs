using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetChangePayments;

public class GetChangePaymentsQuery : IRequest<GetChangePaymentsQueryResult>
{
    public long ApprenticeshipId { get; set; }
}

public class GetChangePaymentsQueryResult
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Uln { get; set; }

    public string CourseName { get; set; }

    public bool FreezeStatus { get; set; }

    public DateTime? PaymentFreezeDate { get; set; }
}

public class GetChangePaymentsQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    ServiceParameters serviceParameters)
    : IRequestHandler<GetChangePaymentsQuery, GetChangePaymentsQueryResult>
{
    public async Task<GetChangePaymentsQueryResult> Handle(GetChangePaymentsQuery request, CancellationToken cancellationToken)
    {
        var innerApiResponse = await apiClient.GetWithResponseCode<GetApprenticeshipResponse>(
            new GetApprenticeshipRequest(request.ApprenticeshipId));

        if (innerApiResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        innerApiResponse.EnsureSuccessStatusCode();
        var apprenticeship = innerApiResponse.Body;

        if (apprenticeship == null || !apprenticeship.CheckParty(serviceParameters))
        {
            return null;
        }

        return new GetChangePaymentsQueryResult
        {
            FirstName = apprenticeship.FirstName,
            LastName = apprenticeship.LastName,
            Uln = apprenticeship.Uln,
            CourseName = apprenticeship.CourseName,
            FreezeStatus = apprenticeship.PaymentFreezeDate.HasValue,
            PaymentFreezeDate = apprenticeship.PaymentFreezeDate
        };
    }
}
