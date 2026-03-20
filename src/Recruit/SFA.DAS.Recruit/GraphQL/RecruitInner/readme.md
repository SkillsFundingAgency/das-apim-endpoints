# GQL
## Install the tools
Make sure you have the StrawberryCake graphql tools installed.

You can install it from a command line; navigate to the root of the SFA.DAS.Recruit project and run the following commands:
```
dotnet new tool-manifest
dotnet tool install StrawberryShake.Tools
```

## Regenerating the schema
If you need to regenerate the client from an updated version of the GQL schema then:

* make sure the Recruit Inner API is running on your local machine
* from a command prompt go into the `SFA.DAS.Recruit/GraphQL/RecruitInner` directory and run the following command:
  * ```dotnet graphql init https://localhost:7288/graphql/ -n RecruitGqlClient -p .```
* build the project

## Adding new queries
To add a new query:
* add a new query in a file with the `.graphql` extension
* build the project

StrawberryShake will then make your new query available on the `RecruitGqlClient` via codegen.

## Fragments
When creating a new query, StrawberryShake generates a completely custom return type
which can make mapping more complicated. By using GraphQL fragments we can simplify this.

If returning the whole vacancy you should can make use of the `AllVacancyFields` fragment,
by _destructuring_ it into the response fields:

```graphql
query ExampleQuery
{
    ...AllVacancyFields
}
```

This will return all fields on the vacancy which can then be mapped into the domain
object using the `GqlVacancyMapper.From` method.

This does mean that when new fields are added/removed to/from the vacancy model,
the `fragments.graphql` file should be updated appropriately along with the mapper.
