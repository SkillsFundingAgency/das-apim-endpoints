﻿using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;
public class AccountHistory
{
    public long AccountId { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime? RemovedDate { get; set; }
}
