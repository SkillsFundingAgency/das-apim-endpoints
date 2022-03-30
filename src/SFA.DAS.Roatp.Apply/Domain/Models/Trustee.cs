﻿using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Roatp.Apply.Domain.Models
{
    public class Trustee
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Trustee(CharityTrustee trustee) =>
            new Trustee()
            {
                Id = trustee.TrusteeId.ToString(),
                Name = trustee.Name,
            };
    }
}
