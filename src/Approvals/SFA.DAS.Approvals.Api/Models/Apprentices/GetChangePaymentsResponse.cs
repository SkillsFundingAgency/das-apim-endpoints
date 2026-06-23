using System;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetChangePayments;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class GetChangePaymentsResponse
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Uln { get; set; }

    public string CourseName { get; set; }

    public bool FreezeStatus { get; set; }

    public DateTime? PaymentFreezeDate { get; set; }

    public static implicit operator GetChangePaymentsResponse(GetChangePaymentsQueryResult source)
    {
        return new GetChangePaymentsResponse
        {
            FirstName = source.FirstName,
            LastName = source.LastName,
            Uln = source.Uln,
            CourseName = source.CourseName,
            FreezeStatus = source.FreezeStatus,
            PaymentFreezeDate = source.PaymentFreezeDate
        };
    }
}
