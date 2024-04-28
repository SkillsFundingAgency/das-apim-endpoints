using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Address;
public class CreateAddressCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string? Uprn { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
}
