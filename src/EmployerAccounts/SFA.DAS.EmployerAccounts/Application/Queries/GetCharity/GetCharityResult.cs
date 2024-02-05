﻿using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCharity
{
    public class GetCharityResult
    {
        public int RegistrationNumber { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Address4 { get; set; }

        public string Address5 { get; set; }

        public string PostCode { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsRemoved { get; set; }

        public static implicit operator GetCharityResult(GetCharityApiResponse source)
        {
            if (source == null)
            {
                return null;
            }

            return new GetCharityResult
            {
                RegistrationNumber = source.RegistrationNumber,
                Name = source.Name,
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                Address4 = source.Address4,
                Address5 = source.Address5,
                PostCode = source.PostCode,
                RegistrationDate = source.RegistrationDate,
                IsRemoved = source.IsRemoved
            };
        }
    }
}
