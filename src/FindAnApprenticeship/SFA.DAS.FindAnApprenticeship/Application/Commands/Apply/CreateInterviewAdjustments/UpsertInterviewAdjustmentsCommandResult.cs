﻿using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateInterviewAdjustments;
public class UpsertInterviewAdjustmentsCommandResult
{
    public Guid Id { get; set; }
    public Domain.Models.Application Application { get; set; }
}
