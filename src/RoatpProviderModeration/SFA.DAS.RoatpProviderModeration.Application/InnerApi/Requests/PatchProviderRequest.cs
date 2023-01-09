using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests
{
    public class PatchProviderRequest : IPatchApiRequest<List<PatchOperation>>
    {
        private const string Replace = "replace";
        private readonly int _ukprn;
        private readonly string _userId;
        private readonly string _userDisplayName;

        public string PatchUrl => $"providers/{_ukprn}?userId={HttpUtility.UrlEncode(_userId)}&userDisplayName={HttpUtility.UrlEncode(_userDisplayName)}";

        public List<PatchOperation> Data { get; set; }

        public PatchProviderRequest(ProviderUpdateModel model)
        {
            _ukprn = model.Ukprn;
            _userId = model.UserId;
            _userDisplayName = model.UserDisplayName;
            Data = BuildDataPatchFromModel(model);
        }

        private List<PatchOperation> BuildDataPatchFromModel(ProviderUpdateModel model)
        {
            var data = new List<PatchOperation>();

            if (model.MarketingInfo != null)
                data.Add(new PatchOperation { Path = "MarketingInfo", Value = model.MarketingInfo, Op = Replace });

            return data;
        }
    }
}
