using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Newtonsoft.Json;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public static class JsonConvertExtensions
    {
        public static AndWhichConstraint<JsonConvertAssertions, T> ShouldBeJson<T>(this string instance)
        {
            return new JsonConvertAssertions(instance).DeserialiseTo<T>();
        }

        public static AndWhichConstraint<JsonConvertAssertions, T> ShouldBeJson<T>(this StringAssertions instance)
        {
            return new JsonConvertAssertions(instance.Subject).DeserialiseTo<T>();
        }

        public class JsonConvertAssertions : ReferenceTypeAssertions<string, JsonConvertAssertions>
        {
            public JsonConvertAssertions(string instance) : base(instance) { }

            protected override string Identifier => "string";

            public AndWhichConstraint<JsonConvertAssertions, T> DeserialiseTo<T>(string because = "", params object[] becauseArgs)
            {
                var deserialised = JsonConvert.DeserializeObject<T>(Subject);

                Execute.Assertion
                    .ForCondition(deserialised != null)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} to contain deserilise to {0}{reason}, but found.",
                        typeof(T).Name);

                return new AndWhichConstraint<JsonConvertAssertions, T>(this, deserialised);
            }
        }
    }
}
