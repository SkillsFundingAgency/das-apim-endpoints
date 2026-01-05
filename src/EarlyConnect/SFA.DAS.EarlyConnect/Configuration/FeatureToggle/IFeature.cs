namespace SFA.DAS.EarlyConnect.Configuration.FeatureToggle
{
    public interface IFeature
    {
        bool IsFeatureEnabled(string feature);
    }
}
