namespace SFA.DAS.EarlyConnect.Configuration.FeatureToggle
{
    public interface IEarlyConnectFeaturesConfiguration 
    {
        bool NorthEastDataSharing { get; set; }
        bool LancashireDataSharing { get; set; }
    }

    public class EarlyConnectFeaturesConfiguration : IEarlyConnectFeaturesConfiguration
    {
        public bool NorthEastDataSharing { get; set; }
        public bool LancashireDataSharing { get; set; }
    }

    public class Feature : IFeature
    {
        private readonly IEarlyConnectFeaturesConfiguration _featuresConfig;
        public Feature(IEarlyConnectFeaturesConfiguration featuresConfig)
        {
            _featuresConfig = featuresConfig;
        }

        public bool IsFeatureEnabled(string feature)
        {
            if (feature.Equals("NorthEastDataSharing")) 
            { 
                return _featuresConfig.NorthEastDataSharing;
            }
            if (feature.Equals("LancashireDataSharing"))
            {
                return _featuresConfig.LancashireDataSharing;
            }
            return false;
        }
    }
}
