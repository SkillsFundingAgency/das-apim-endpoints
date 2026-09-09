using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

public class PostCreateReservationRequest(CreateReservationRequest data) : IPostApiRequest
{
    public string PostUrl => $"api/accounts/{AccountId}/reservations";
    public long AccountId { get; } = data.AccountId;
    public object Data { get; set; } = data;
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