﻿using System.Collections.Generic;
using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class PatchProviderCourseRequest : IPatchApiRequest<List<PatchOperation>>
    {
        private const string Replace = "replace";
        private readonly int _ukprn;
        private readonly int _larsCode;
        private readonly string _userId;
        private readonly string _userDisplayName;

        public string PatchUrl => $"providers/{_ukprn}/courses/{_larsCode}?userId={HttpUtility.UrlEncode(_userId)}&userDisplayName={HttpUtility.UrlEncode(_userDisplayName)}";

        public List<PatchOperation> Data { get; set; }

        public PatchProviderCourseRequest(ProviderCourseUpdateModel model)
        {
            _ukprn = model.Ukprn;
            _larsCode = model.LarsCode;
            _userId = model.UserId; 
            _userDisplayName = model.UserDisplayName;
            Data = BuildDataPatchFromModel(model);
        }

        private List<PatchOperation> BuildDataPatchFromModel(ProviderCourseUpdateModel model)
        {
            var data = new List<PatchOperation>();

            if (model.ContactUsEmail != null)
                data.Add(new PatchOperation { Path = "ContactUsEmail", Value = model.ContactUsEmail, Op=Replace });

            if (model.ContactUsPageUrl != null)
                data.Add(new PatchOperation { Path = "ContactUsPageUrl", Value = model.ContactUsPageUrl, Op=Replace });

            if (model.ContactUsPhoneNumber != null)
                data.Add(new PatchOperation { Path = "ContactUsPhoneNumber", Value = model.ContactUsPhoneNumber, Op=Replace });

            if (model.StandardInfoUrl != null)
                data.Add(new PatchOperation { Path = "StandardInfoUrl", Value = model.StandardInfoUrl, Op=Replace });

            if (model.IsApprovedByRegulator != null)
                data.Add(new PatchOperation { Path = "IsApprovedByRegulator", Value = model.IsApprovedByRegulator, Op=Replace });
            
            return data;
        }
    }
}
