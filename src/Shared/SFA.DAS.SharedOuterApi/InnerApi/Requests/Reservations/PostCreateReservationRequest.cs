using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;

public class PostCreateReservationRequest : IPostApiRequest
{
    public string PostUrl => $"api/accounts/{AccountId}/reservations";
    public long AccountId { get; }
    public object Data { get; set; }

    public PostCreateReservationRequest(CreateReservationRequest data)
    {
        Data = data;
        AccountId = data.AccountId;
    }
}

public class CreateReservationRequest
{
    public Guid Id { get; set; }
    public long AccountId { get; set; }
    public DateTime? StartDate { get; set; }
    public string CourseId { get; set; }
    public uint? ProviderId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string AccountLegalEntityName { get; set; }
    public bool IsLevyAccount { get; set; }
    public long? TransferSenderAccountId { get; set; }
    public Guid? UserId { get; set; }
}