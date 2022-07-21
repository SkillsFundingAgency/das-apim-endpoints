using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class PatchProviderCourseRequest : IPatchApiRequest<List<PatchOperation>>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string PatchUrl => $"providers/{Ukprn}/courses/{LarsCode}";

        public List<PatchOperation> Data { get; set; }



        public PatchProviderCourseRequest(ProviderCourseUpdateModel model)
        {
            Ukprn = model.Ukprn;
            LarsCode = model.LarsCode;
            UserId = model.UserId;
            Data = BuildDataPatchFromModel(model);
        }

        private List<PatchOperation> BuildDataPatchFromModel(ProviderCourseUpdateModel model)
        {
            var data = new List<PatchOperation>();

            if (model.ContactUsEmail != null)
            {
                data.Add(new PatchOperation { Path = "ContactUsEmail", Value = model.ContactUsEmail });
            }
            
            if (model.ContactUsPageUrl != null)
            {
                data.Add(new PatchOperation { Path = "ContactUsPageUrl", Value = model.ContactUsPageUrl });
            }
            
            if (model.ContactUsPhoneNumber != null)
            {
                data.Add(new PatchOperation
                    { Path = "ContactUsPhoneNumber", Value = model.ContactUsPhoneNumber });
            }
            
            if (model.StandardInfoUrl != null)
            {
                data.Add(new PatchOperation { Path = "StandardInfoUrl", Value = model.StandardInfoUrl });
            }
            
            if (model.IsApprovedByRegulator != null)
            {
                data.Add(new PatchOperation
                    { Path = "IsApprovedByRegulator", Value = model.IsApprovedByRegulator });
            }
            
            return data;
        }
    }
}
