﻿using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.PhoneNumber;
public class CreatePhoneNumberCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
