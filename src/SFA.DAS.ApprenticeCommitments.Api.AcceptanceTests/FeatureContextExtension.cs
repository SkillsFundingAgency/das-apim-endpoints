using System;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public static class FeatureContextExtension
    {
        public static T GetOrAdd<T>(this FeatureContext feature, [CallerMemberName] string name = "") where T : class, new()
            => GetOrAdd(feature, () => new T(), name);

        public static T GetOrAdd<T>(this FeatureContext feature, Func<T> creator, [CallerMemberName] string name = "") where T : class
        {
            if (!feature.ContainsKey(name))
                feature[name] = creator();
            return feature[name] as T;
        }
    }
}